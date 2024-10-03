using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Clan;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Security.Claims;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private static readonly string[] ClanParamFields = new string[] {
            "clan_level", "member_num", "master_id", "system_restriction",
            "is_base_release", "can_base_release", "total_clan_point", "money_clan_point", "next_clan_point",
            "name", "short_name", "emblem_mark_type", "emblem_base_type", "emblem_main_color",
            "emblem_sub_color", "motto", "active_days", "active_time", "characteristic",
            "is_publish", "comment", "board_message", "created"
        };

        private static readonly string[] ClanMembershipFields = new string[]
        {
            "character_id", "clan_id", "rank", "permission"
        };

        private readonly string SqlInsertClanParam = $"INSERT INTO \"ddon_clan_param\" ({BuildQueryField(ClanParamFields)}) VALUES ({BuildQueryInsert(ClanParamFields)});";
        private readonly string SqlDeleteClanParam = "DELETE FROM \"ddon_clan_param\" WHERE \"clan_id\"=@clan_id;";
        private readonly string SqlUpdateClanParam = $"UPDATE \"ddon_clan_param\" SET {BuildQueryUpdate(ClanParamFields)} WHERE \"clan_id\" = @clan_id;";
        private readonly string SqlSelectClanParamById = $"SELECT \"clan_id\", {BuildQueryField(ClanParamFields)} FROM \"ddon_clan_param\" WHERE \"clan_id\"=@clan_id;";

        private readonly string SqlIncrementClanMemberNum = "UPDATE \"ddon_clan_param\" SET \"member_num\" = \"member_num\" + @value WHERE \"clan_id\" = @clan_id;";

        private readonly string SqlInsertClanMembership = $"INSERT INTO \"ddon_clan_membership\" ({BuildQueryField(ClanMembershipFields)}) VALUES ({BuildQueryInsert(ClanMembershipFields)});";
        private readonly string SqlSelectClanMembershipByCharacterId = $"SELECT {BuildQueryField(ClanMembershipFields)} FROM \"ddon_clan_membership\" WHERE \"character_id\"=@character_id;";
        private readonly string SqlDeleteClanMembership = "DELETE FROM \"ddon_clan_membership\" WHERE \"character_id\"=@character_id;";
        private readonly string SqlUpdateClanMembership = $"UPDATE \"ddon_clan_membership\" SET {BuildQueryUpdate(ClanMembershipFields)} WHERE \"character_id\" = @character_id";

        private readonly string SqlCDataClanMemberInfoList = "SELECT \"ddon_clan_membership\".\"character_id\", \"ddon_clan_membership\".\"rank\", \"ddon_clan_membership\".\"permission\", \"ddon_character\".\"first_name\", \"ddon_character\".\"last_name\", \"ddon_character_job_data\".\"job\", \"ddon_character_job_data\".\"lv\" "
            + "FROM \"ddon_clan_membership\" "
            + "INNER JOIN \"ddon_character\" ON \"ddon_clan_membership\".\"character_id\" = \"ddon_character\".\"character_id\" AND \"ddon_clan_membership\".\"clan_id\" = @clan_id "
            + "INNER JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "INNER JOIN \"ddon_character_job_data\" ON \"ddon_character_job_data\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" AND \"ddon_character_job_data\".\"job\" = \"ddon_character_common\".\"job\";";
        private readonly string SqlCDataClanMemberInfo = "SELECT \"ddon_clan_membership\".\"character_id\", \"ddon_clan_membership\".\"rank\", \"ddon_clan_membership\".\"permission\", \"ddon_character\".\"first_name\", \"ddon_character\".\"last_name\", \"ddon_character_job_data\".\"job\", \"ddon_character_job_data\".\"lv\" "
            + "FROM \"ddon_clan_membership\" "
            + "INNER JOIN \"ddon_character\" ON \"ddon_clan_membership\".\"character_id\" = \"ddon_character\".\"character_id\" AND \"ddon_character\".\"character_id\" = @character_id "
            + "INNER JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "INNER JOIN \"ddon_character_job_data\" ON \"ddon_character_job_data\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" AND \"ddon_character_job_data\".\"job\" = \"ddon_character_common\".\"job\";";

        public bool CreateClan(CDataClanParam clanParam)
        {
            return ExecuteInTransaction(conn =>
            {
                ExecuteNonQuery(
                    conn,
                    SqlInsertClanParam,
                    command =>
                    {
                        AddParameter(command, clanParam);
                    },
                    out long clanId
                );
                clanParam.ClanServerParam.ID = (uint)clanId;

                InsertClanMember(clanParam.ClanServerParam.MasterInfo, clanParam.ClanServerParam.ID, conn);
            });
        }

        public CDataClanParam SelectClan(uint clanId, DbConnection? connectionIn = null)
        {
            CDataClanParam clanParam = new CDataClanParam();

            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                uint masterId = 0;
                ExecuteReader(
                connection,
                SqlSelectClanParamById,
                command =>
                {
                    AddParameter(command, "@clan_id", clanId);
                },
                reader =>
                {
                    if (reader.Read())
                    {
                        clanParam.ClanUserParam = ReadClanUserParam(reader);
                        clanParam.ClanServerParam = ReadClanServerParam(reader);
                        masterId = GetUInt32(reader, "master_id");
                    }
                });

                if (masterId > 0)
                {
                    var master = GetClanMember(masterId);
                    if (master is not null)
                    {
                        clanParam.ClanServerParam.MasterInfo = master;
                    }
                    else
                    {
                        ExecuteNonQuery(
                            connection,
                            "UPDATE \"ddon_clan_param\" SET \"master_id\" = 0 WHERE \"clan_id\" = @clan_id;",
                            command =>
                            {
                                AddParameter(command, "@clan_id", clanId);
                            }
                        );
                    }
                }
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
            return clanParam;
        }

        public uint SelectClanMembershipByCharacterId(uint characterId, DbConnection? connectionIn = null)
        {
            uint clanId = 0;
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                ExecuteReader(connection,
                    SqlSelectClanMembershipByCharacterId,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                {
                    if (reader.Read())
                    {
                        clanId = GetUInt32(reader, "clan_id");
                    }
                });
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }

            return clanId;
        }

        public List<CDataClanMemberInfo> GetClanMemberList(uint clanId, DbConnection? connectionIn = null)
        {
            List<CDataClanMemberInfo> list = new();
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                ExecuteReader(connection,
                    SqlCDataClanMemberInfoList,
                    command => { AddParameter(command, "@clan_id", clanId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            CDataClanMemberInfo info = new CDataClanMemberInfo()
                            {
                                Rank = (ClanMemberRank)GetUInt32(reader, "rank"),
                                Permission = GetUInt32(reader, "permission"),
                                CharacterListElement = new()
                                {
                                    CommunityCharacterBaseInfo = new()
                                    {
                                        CharacterId = GetUInt32(reader, "character_id"),
                                        CharacterName = new()
                                        {
                                            FirstName = GetString(reader, "first_name"),
                                            LastName = GetString(reader, "last_name")
                                        }
                                    },
                                    CurrentJobBaseInfo = new()
                                    {
                                        Job = (JobId)GetByte(reader, "job"),
                                        Level = GetByte(reader, "lv")
                                    }
                                }
                            };

                            list.Add(info);
                        }
                    });
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }

            return list;
        }

        public CDataClanMemberInfo GetClanMember(uint characterId, DbConnection? connectionIn = null)
        {
            CDataClanMemberInfo member = null;
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                ExecuteReader(connection,
                    SqlCDataClanMemberInfo,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        if (reader.Read())
                        {
                            member = new CDataClanMemberInfo()
                            {
                                Rank = (ClanMemberRank)GetUInt32(reader, "rank"),
                                Permission = GetUInt32(reader, "permission"),
                                CharacterListElement = new()
                                {
                                    CommunityCharacterBaseInfo = new()
                                    {
                                        CharacterId = GetUInt32(reader, "character_id"),
                                        CharacterName = new()
                                        {
                                            FirstName = GetString(reader, "first_name"),
                                            LastName = GetString(reader, "last_name")
                                        }
                                    },
                                    CurrentJobBaseInfo = new()
                                    {
                                        Job = (JobId)GetByte(reader, "job"),
                                        Level = GetByte(reader, "lv")
                                    }
                                }
                            };
                        }
                    });
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }

            return member;
        }

        public ClanName GetClanNameByClanId(uint clanId, DbConnection? connectionIn = null)
        {
            ClanName clanName = new ClanName();

            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                ExecuteReader(
                connection,
                SqlSelectClanParamById,
                command =>
                {
                    AddParameter(command, "@clan_id", clanId);
                },
                reader =>
                {
                    if (reader.Read())
                    {
                        clanName.Name = GetString(reader, "name");
                        clanName.ShortName = GetString(reader, "short_name");
                    }
                });
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }

            return clanName;
        }

        public bool UpdateClan(CDataClanParam clan, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                return ExecuteNonQuery(connection, SqlUpdateClanParam, command =>
                {
                    AddParameter(command, clan);
                }) == 1;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public bool DeleteClan(CDataClanParam clan, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                return ExecuteNonQuery(connection, SqlDeleteClanParam, command =>
                {
                    AddParameter(command, clan);
                }) == 1;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public bool IncrementClanMemberNum(int value, uint clanId, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                return ExecuteNonQuery(connection, SqlIncrementClanMemberNum, command =>
                {
                    AddParameter(command, "@clan_id", clanId);
                    AddParameter(command, "@value", value);
                }) == 1;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public bool InsertClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                var memberInsert = ExecuteNonQuery(
                    connection,
                    SqlInsertClanMembership,
                    command =>
                    {
                        AddParameter(command, memberInfo, clanId);
                    }
                ) == 1;
                var incrementNum = IncrementClanMemberNum(1, clanId, connection);
                return memberInsert && incrementNum;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public bool DeleteClanMember(uint characterId, uint clanId, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                var memberInsert = ExecuteNonQuery(
                    connection,
                    SqlDeleteClanMembership,
                    command =>
                    {
                        AddParameter(command, "@character_id", characterId);
                    }
                ) == 1;
                var incrementNum = IncrementClanMemberNum(-1, clanId, connection);
                return memberInsert && incrementNum;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public bool UpdateClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                var memberUpdate = ExecuteNonQuery(
                    connection,
                    SqlUpdateClanMembership,
                    command =>
                    {
                        AddParameter(command, memberInfo, clanId);
                    }
                ) == 1;
                return memberUpdate;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        private void AddParameter(TCom command, CDataClanParam clanParam)
        {

            AddParameter(command, "@clan_id", clanParam.ClanServerParam.ID);
            AddParameter(command, "@clan_level", clanParam.ClanServerParam.Lv);
            AddParameter(command, "@member_num", clanParam.ClanServerParam.MemberNum);
            AddParameter(command, "@master_id", clanParam.ClanServerParam.MasterInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId);
            AddParameter(command, "@system_restriction", clanParam.ClanServerParam.IsSystemRestriction);
            AddParameter(command, "@is_base_release", clanParam.ClanServerParam.IsClanBaseRelease);
            AddParameter(command, "@can_base_release", clanParam.ClanServerParam.CanClanBaseRelease);
            AddParameter(command, "@total_clan_point", clanParam.ClanServerParam.TotalClanPoint);
            AddParameter(command, "@money_clan_point", clanParam.ClanServerParam.MoneyClanPoint);
            AddParameter(command, "@next_clan_point", clanParam.ClanServerParam.NextClanPoint);
            AddParameter(command, "@name", clanParam.ClanUserParam.Name);
            AddParameter(command, "@short_name", clanParam.ClanUserParam.ShortName);
            AddParameter(command, "@emblem_mark_type", clanParam.ClanUserParam.EmblemMarkType);
            AddParameter(command, "@emblem_base_type", clanParam.ClanUserParam.EmblemBaseType);
            AddParameter(command, "@emblem_main_color", clanParam.ClanUserParam.EmblemBaseMainColor);
            AddParameter(command, "@emblem_sub_color", clanParam.ClanUserParam.EmblemBaseSubColor);
            AddParameter(command, "@motto", clanParam.ClanUserParam.Motto);
            AddParameter(command, "@active_days", clanParam.ClanUserParam.ActiveDays);
            AddParameter(command, "@active_time", clanParam.ClanUserParam.ActiveTime);
            AddParameter(command, "@characteristic", clanParam.ClanUserParam.Characteristic);
            AddParameter(command, "@is_publish", clanParam.ClanUserParam.IsPublish);
            AddParameter(command, "@comment", clanParam.ClanUserParam.Comment);
            AddParameter(command, "@board_message", clanParam.ClanUserParam.BoardMessage);
            AddParameter(command, "@created", clanParam.ClanUserParam.Created);
        }

        private void AddParameter(TCom command, CDataClanMemberInfo memberInfo, uint clanId)
        {
            AddParameter(command, "@character_id", memberInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId);
            AddParameter(command, "@clan_id", clanId);
            AddParameter(command, "@rank", (uint)memberInfo.Rank);
            AddParameter(command, "@permission", memberInfo.Permission);
        }

        private CDataClanUserParam ReadClanUserParam(DbDataReader reader)
        {
            CDataClanUserParam userParam = new()
            {
                Name = GetString(reader, "name"),
                ShortName = GetString(reader, "short_name"),
                EmblemMarkType = GetByte(reader, "emblem_mark_type"),
                EmblemBaseType = GetByte(reader, "emblem_base_type"),
                EmblemBaseMainColor = GetByte(reader, "emblem_main_color"),
                EmblemBaseSubColor = GetByte(reader, "emblem_sub_color"),
                Motto = GetUInt32(reader, "motto"),
                ActiveDays = GetUInt32(reader, "active_days"),
                ActiveTime = GetUInt32(reader, "active_time"),
                Characteristic = GetUInt32(reader, "characteristic"),
                IsPublish = GetBoolean(reader, "is_publish"),
                Comment = GetString(reader, "comment"),
                BoardMessage = GetString(reader, "board_message"),
                Created = GetInt64(reader, "created")
            };

            return userParam;
        }

        private CDataClanServerParam ReadClanServerParam(DbDataReader reader)
        {
            CDataClanServerParam serverParam = new()
            {
                ID = GetUInt32(reader, "clan_id"),
                Lv = GetUInt16(reader, "clan_level"),
                MemberNum = GetUInt16(reader, "member_num"),
                IsSystemRestriction = GetBoolean(reader, "system_restriction"),
                IsClanBaseRelease = GetBoolean(reader, "is_base_release"),
                CanClanBaseRelease = GetBoolean(reader, "can_base_release"),
                TotalClanPoint = GetUInt32(reader, "total_clan_point"),
                MoneyClanPoint = GetUInt32(reader, "money_clan_point"),
                NextClanPoint = GetUInt32(reader, "next_clan_point")
            };

            return serverParam;
        }
    }
}
