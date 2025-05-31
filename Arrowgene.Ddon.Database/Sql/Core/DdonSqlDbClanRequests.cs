using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Data.Common;
using System;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public partial class DdonSqlDb : SqlDb
    {
        /* ddon_clan_requests */
        protected static readonly string[] ClanRequestFields = new[]
        {
            "clan_id", "requester_character_id", "created", "approved"
        };
        
        private readonly string SqlInsertClanRequest = $"""
            INSERT INTO ddon_clan_requests ({BuildQueryField(ClanRequestFields)})
            VALUES ({BuildQueryInsert(ClanRequestFields)});
        """;

        private readonly string SqlSelectClanRequestsByCharacter = $"""
            SELECT req."clan_id", req."requester_character_id", req."created", req."approved", chara."first_name", chara."last_name", param."name"
            FROM ddon_clan_requests AS req
            INNER JOIN ddon_character AS chara ON req.requester_character_id = chara.character_id
            INNER JOIN ddon_clan_param AS param ON param.clan_id = req.clan_id
            WHERE req.requester_character_id = @requester_character_id
            AND req.approved = 0;
        """;

        private readonly string SqlSelectApprovedClanRequestsByCharacter = $"""
            SELECT req."clan_id", req."requester_character_id", req."created", req."approved", chara."first_name", chara."last_name", param."name"
            FROM ddon_clan_requests AS req
            INNER JOIN ddon_character AS chara ON req.requester_character_id = chara.character_id
            INNER JOIN ddon_clan_param AS param ON param.clan_id = req.clan_id
            WHERE req.requester_character_id = @requester_character_id
            AND req.approved = 1;
        """;
            
        private readonly string SqlSelectClanRequestsByClan = $"""
            SELECT req."clan_id", req."requester_character_id", req."created", req."approved", chara."first_name", chara."last_name", param."name"
            FROM ddon_clan_requests AS req
            INNER JOIN ddon_character AS chara ON req.requester_character_id = chara.character_id
            INNER JOIN ddon_clan_param AS param ON param.clan_id = req.clan_id
            WHERE req.clan_id = @clan_id
            AND req.approved = 0;
        """;

        private readonly string SqlSetClanRequestApproved = $"""
            UPDATE ddon_clan_requests
            SET approved = 1
            WHERE requester_character_id = @requester_character_id;
        """;

        private readonly string SqlDeleteClanRequestByCharacter = $"""
            DELETE FROM ddon_clan_requests
            WHERE requester_character_id = @requester_character_id;
        """;

        private readonly string SqlDeleteOldClanRequests = $"""
            DELETE FROM ddon_clan_requests
            WHERE created < @date_filter;
        """;


        public override bool InsertClanRequest(uint clanId, uint characterId, DbConnection connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlInsertClanRequest, command =>
                {
                    AddParameter(command, "clan_id", clanId);
                    AddParameter(command, "requester_character_id", characterId);
                    AddParameter(command, "created", DateTime.Now);
                    AddParameter(command, "approved", 0);
                }) == 1;
            });
        }

        public override List<CDataClanJoinRequest> GetClanRequestsByCharacter(uint characterId, DbConnection? connectionIn = null)
        {
            List<CDataClanJoinRequest> results = new();
            ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteReader(connection, SqlSelectClanRequestsByCharacter, command =>
                {
                    AddParameter(command, "requester_character_id", characterId);
                },
                reader =>
                { 
                    while (reader.Read())
                    {
                        results.Add(ReadClanRequestData(reader));
                    }
                });
            });

            return results;
        }
        public override List<CDataClanJoinRequest> GetApprovedClanRequestsByCharacter(uint characterId, DbConnection? connectionIn = null)
        {
            List<CDataClanJoinRequest> results = new();
            ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteReader(connection, SqlSelectApprovedClanRequestsByCharacter, command =>
                {
                    AddParameter(command, "requester_character_id", characterId);
                },
                reader =>
                { 
                    while (reader.Read())
                    {
                        results.Add(ReadClanRequestData(reader));
                    }
                });
            });

            return results;
        }

        public override List<CDataClanJoinRequest> GetClanRequestsByClan(uint clanId, DbConnection? connectionIn = null)
        {
            List<CDataClanJoinRequest> results = new();
            ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteReader(connection, SqlSelectClanRequestsByClan, command =>
                {
                    AddParameter(command, "clan_id", clanId);
                },
                reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(ReadClanRequestData(reader));
                    }
                });
            });

            return results;
        }

        public override bool SetClanRequestApproved(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlSetClanRequestApproved, command =>
                {
                    AddParameter(command, "requester_character_id", characterId);
                }) == 1;
            });
        }

        public override bool DeleteClanRequestByCharacter(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlDeleteClanRequestByCharacter, command =>
                {
                    AddParameter(command, "requester_character_id", characterId);
                }) == 1;
            });
        }

        public override bool DeleteOldClanRequests(uint minDays = 30, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlDeleteOldClanRequests, command =>
                {
                    AddParameter(command, "date_filter", DateTime.Now.AddDays(-minDays));
                }) == 1;
            });
        }

        private CDataClanJoinRequest ReadClanRequestData(DbDataReader reader)
        {
            CDataClanJoinRequest result = new()
            {
                ClanId = GetUInt32(reader, "clan_id"),
                ClanName = GetString(reader, "name"),
                BaseInfo = new CDataCommunityCharacterBaseInfo()
                {
                    CharacterId = GetUInt32(reader, "requester_character_id"),
                    CharacterName = new CDataCharacterName
                    {
                        FirstName = GetString(reader, "first_name"),
                        LastName = GetString(reader, "last_name")
                    }
                },
                CreateTime = GetDateTime(reader, "created")
            };

            return result;
        }
    }
}
