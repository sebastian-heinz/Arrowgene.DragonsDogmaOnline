using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Clan;
using System.Data.Common;

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
        private readonly string SqlDeleteClanParam = "DELETE FROM \"ddon_clam_param\" WHERE \"clan_id\"=@clan_id;";
        private readonly string SqlUpdateClanParam = $"UPDATE \"ddon_clan_param\" SET {BuildQueryUpdate(ClanParamFields)} WHERE \"clan_id\" = @clan_id;";
        private readonly string SqlSelectClanParamById = $"SELECT \"clan_id\", {BuildQueryField(ClanParamFields)} FROM \"ddon_clan_param\" WHERE \"clan_id\"=@clan_id;";

        private readonly string SqlInsertClanMembership = $"INSERT INTO \"ddon_clan_membership\" ({BuildQueryField(ClanMembershipFields)}) VALUES ({BuildQueryInsert(ClanMembershipFields)});";
        private readonly string SqlSelectClanMembershipByCharacterId = $"SELECT {BuildQueryField(ClanMembershipFields)} FROM \"ddon_clan_membership\" WHERE \"character_id\"=@character_id;";

        public bool CreateClan(ClanParam clanParam)
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
                clanParam.ID = (uint)clanId;

                ExecuteNonQuery(
                    conn,
                    SqlInsertClanMembership,
                    command =>
                    {
                        AddParameter(command, clanParam.MasterInfo, clanParam.ID);
                    }
                );
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
                    // TODO: Fetch clan master with awful join.
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

        private void AddParameter(TCom command, ClanParam clanParam)
        {
            AddParameter(command, "@clan_id", clanParam.ID);
            AddParameter(command, "@clan_level", clanParam.Lv);
            AddParameter(command, "@member_num", clanParam.MemberNum);
            AddParameter(command, "@master_id", clanParam.MasterInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId);
            AddParameter(command, "@system_restriction", clanParam.IsSystemRestriction);
            AddParameter(command, "@is_base_release", clanParam.IsClanBaseRelease);
            AddParameter(command, "@can_base_release", clanParam.CanClanBaseRelease);
            AddParameter(command, "@total_clan_point", clanParam.TotalClanPoint);
            AddParameter(command, "@money_clan_point", clanParam.MoneyClanPoint);
            AddParameter(command, "@next_clan_point", clanParam.NextClanPoint);
            AddParameter(command, "@name", clanParam.Name);
            AddParameter(command, "@short_name", clanParam.ShortName);
            AddParameter(command, "@emblem_mark_type", clanParam.EmblemMarkType);
            AddParameter(command, "@emblem_base_type", clanParam.EmblemBaseType);
            AddParameter(command, "@emblem_main_color", clanParam.EmblemBaseMainColor);
            AddParameter(command, "@emblem_sub_color", clanParam.EmblemBaseSubColor);
            AddParameter(command, "@motto", clanParam.Motto);
            AddParameter(command, "@active_days", clanParam.ActiveDays);
            AddParameter(command, "@active_time", clanParam.ActiveTime);
            AddParameter(command, "@characteristic", clanParam.Characteristic);
            AddParameter(command, "@is_publish", clanParam.IsPublish);
            AddParameter(command, "@comment", clanParam.Comment);
            AddParameter(command, "@board_message", clanParam.BoardMessage);
            AddParameter(command, "@created", clanParam.Created);
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
