using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public partial class DdonSqlDb : SqlDb
    {
        protected static readonly string[] GroupChatFields = new[]
        {
            "character_id", "group_id",
        };

        protected static readonly string[] GroupChatGroupsFields = new[]
        {
            /*"group_id",*/ "group_name", "group_desc", "prevent_deletion"
        };

        private static readonly string SqlSelectNextGroupId = "SELECT COALESCE(MAX(\"group_id\"), 0) + 1 AS next_group_id FROM \"ddon_group_chat\";";
        private static readonly string SqlSelectGroupChatMembers = @"
            SELECT 
                c.character_id,
	            c.first_name, 
	            c.last_name, 
	            j.job, 
	            j.lv, 
	            mp.comment,
                cp.short_name AS clan_name,
                COALESCE(conn.server_id, 0) AS server_id
            FROM ddon_group_chat gc
            INNER JOIN ddon_character c ON c.character_id = gc.character_id
            INNER JOIN ddon_character_common cc ON cc.character_common_id = c.character_common_id
            INNER JOIN ddon_character_job_data j ON j.character_common_id = cc.character_common_id AND j.job = cc.job
            INNER JOIN ddon_character_matching_profile mp ON mp.character_id = c.character_id
            LEFT JOIN ddon_clan_membership cm ON cm.character_id = c.character_id
            LEFT JOIN ddon_clan_param cp ON cp.clan_id = cm.clan_id
            LEFT JOIN ddon_connection conn ON conn.account_id = c.account_id
            WHERE gc.group_id = @group_id
            LIMIT 100;";

        private static readonly string SqlSelectGroupChatByCharacterId =
            @"SELECT  
                g.group_id,
                g.group_name
            FROM ddon_group_chat m
            LEFT JOIN ddon_group_chat_groups g ON m.group_id = g.group_id
            WHERE character_id = @character_id;";
        private static readonly string SqlInsertGroupChatMember =
            $"""
            INSERT INTO "ddon_group_chat" ("character_id", "group_id") 
            VALUES (@character_id, @group_id) 
            ON CONFLICT (character_id) 
            DO UPDATE SET "group_id" = EXCLUDED."group_id";
            """;
        private static readonly string SqlDeleteGroupChatMember =
            $"DELETE FROM \"ddon_group_chat\" WHERE \"character_id\" = @character_id;";

        private static readonly string SqlSelectGroupChatGroups =
            @"
                SELECT
                    g.group_id,
                    g.group_name,
                    g.group_desc,
                    COUNT(m.character_id) AS total_count,
                    SUM(CASE WHEN conn.server_id IS NOT NULL THEN 1 ELSE 0 END) AS active_count
                FROM ddon_group_chat_groups g
                LEFT JOIN ddon_group_chat m ON m.group_id = g.group_id
                LEFT JOIN ddon_character c ON m.character_id = c.character_id
                LEFT JOIN ddon_connection conn ON conn.account_id = c.account_id
                GROUP BY g.group_id
                ORDER BY g.group_id;
            ";
        private static readonly string SqlSelectGroupChatGroupById =
            @"
                SELECT
                    g.group_id,
                    g.group_name,
                    g.group_desc
                FROM ddon_group_chat_groups g
                WHERE g.group_id = @group_id;
            ";
        private static readonly string SqlSelectGroupChatGroupByName =
            @"
                SELECT
                    g.group_id,
                    g.group_name,
                    g.group_desc
                FROM ddon_group_chat_groups g
                WHERE g.group_name = @group_name;
            ";

        private static readonly string SqlInsertGroupChatGroup =
            $"INSERT INTO \"ddon_group_chat_groups\" ({BuildQueryField(GroupChatGroupsFields)}) VALUES ({BuildQueryInsert(GroupChatGroupsFields)});";
        private static readonly string SqlDisbandGroupChatGroup =
            $"DELETE FROM \"ddon_group_chat_groups\" WHERE \"group_id\" = @group_id;";
        private static readonly string SqlPruneGroupChatGroups = @"
            DELETE FROM ddon_group_chat_groups 
            WHERE prevent_deletion = FALSE
                AND NOT EXISTS (
                    SELECT 1
                    FROM ddon_group_chat
                    WHERE ddon_group_chat.group_id = ddon_group_chat_groups.group_id
                );
        ";

        public override ulong SelectNextGroupChatId(DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                ulong result = 1;

                ExecuteReader(connection, SqlSelectNextGroupId,
                    command => { }, reader =>
                    {
                        if (reader.Read()) result = GetUInt64(reader, "next_group_id");
                    });

                return result;
            });
        }

        public override (ulong Id, string Name) SelectGroupChatId(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                (ulong Id, string Name) result = (0, "");

                ExecuteReader(connection, SqlSelectGroupChatByCharacterId,
                    command => {
                        AddParameter(command, "@character_id", characterId);
                    }, reader =>
                    {
                        if (reader.Read())
                        {
                            ulong groupId = GetUInt64(reader, "group_id");
                            string groupName = GetStringNullable(reader, "group_name") ?? "";

                            result = (groupId, groupName);
                        }
                    });

                return result;
            });
        }

        public override (ulong Id, string Name) SelectGroupChatName(string groupName, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                (ulong Id, string Name) result = (0, "");

                ExecuteReader(connection, SqlSelectGroupChatGroupByName,
                    command => {
                        AddParameter(command, "@group_name", groupName);
                    }, reader =>
                    {
                        if (reader.Read())
                        {
                            ulong groupId = GetUInt64(reader, "group_id");
                            string groupName = GetStringNullable(reader, "group_name") ?? "";

                            result = (groupId, groupName);
                        }
                    });

                return result;
            });
        }

        public override List<CDataCharacterListElement> SelectGroupChatMembers(ulong groupId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                List<CDataCharacterListElement> results = [];

                ExecuteReader(connection, SqlSelectGroupChatMembers,
                    command => { AddParameter(command, "@group_id", groupId); },
                    reader => {

                        while (reader.Read())
                        {
                            CDataCharacterListElement characterListElement = new();

                            characterListElement.CommunityCharacterBaseInfo.CharacterId = GetUInt32(reader, "character_id");
                            characterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = GetString(reader, "first_name");
                            characterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = GetString(reader, "last_name");
                            characterListElement.CommunityCharacterBaseInfo.ClanName = GetStringNullable(reader, "clan_name") ?? string.Empty;
                            characterListElement.CurrentJobBaseInfo.Job = (JobId)GetByte(reader, "job");
                            characterListElement.CurrentJobBaseInfo.Level = GetByte(reader, "lv");
                            characterListElement.EntryJobBaseInfo = characterListElement.CurrentJobBaseInfo;
                            characterListElement.MatchingProfile = GetString(reader, "comment");
                            characterListElement.ServerId = GetUInt16(reader, "server_id");
                            characterListElement.OnlineStatus = characterListElement.ServerId > 0 ? OnlineStatus.Online : OnlineStatus.Offline;

                            if (characterListElement.ServerId != 0)
                            {
                                results.Add(characterListElement);
                            }
                        }
                    });

                return results;
            });
        }

        public override bool InsertGroupChatMember(uint characterId, ulong groupId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlInsertGroupChatMember, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "group_id", groupId);
                }) == 1;
            });
        }

        public override bool DeleteGroupChatMember(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlDeleteGroupChatMember, command => { 
                    AddParameter(command, "@character_id", characterId);
                }) == 1;
            });
        }

        public override bool DisbandGroupChat(ulong groupId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlDisbandGroupChatGroup, command => { AddParameter(command, "@group_id", groupId); }) > 0;
            });
        }

        public override long InsertGroupChatGroup(string groupName, string groupDesc, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteNonQuery(connection, SqlInsertGroupChatGroup, command =>
                {
                    AddParameter(command, "group_name", groupName);
                    AddParameter(command, "group_desc", groupDesc);
                    AddParameter(command, "prevent_deletion", false);
                }, out long groupId);

                return groupId;
            });
        }

        public override int PruneGroupChatGroups(DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlPruneGroupChatGroups, command => { });
            });
        }

        public override Dictionary<string, (ulong Id, uint Count, uint CountTotal, string Desc)> SelectGroupChatGroups(DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                Dictionary<string, (ulong Id, uint Count, uint CountTotal, string Desc)> results = [];

                ExecuteReader(connection, SqlSelectGroupChatGroups,
                    command => { },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            CDataCharacterListElement characterListElement = new();

                            var groupName = GetString(reader, "group_name");
                            var groupDesc = GetString(reader, "group_desc");
                            var groupId = GetUInt64(reader, "group_id");
                            var activeCount = GetUInt32(reader, "active_count");
                            var totalCount = GetUInt32(reader, "total_count");

                            results.Add(groupName, (groupId, activeCount, totalCount, groupDesc));
                        }
                    });

                return results;
            });
        }
    }
}
