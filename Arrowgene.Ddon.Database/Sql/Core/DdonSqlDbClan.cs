using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Clan;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string[] ClanParamFields = new[]
    {
        "clan_level", "member_num", "master_id", "system_restriction",
        "is_base_release", "can_base_release", "total_clan_point", "money_clan_point",
        "name", "short_name", "emblem_mark_type", "emblem_base_type", "emblem_main_color",
        "emblem_sub_color", "motto", "active_days", "active_time", "characteristic",
        "is_publish", "comment", "board_message", "created"
    };

    private static readonly string[] ClanSearchResultFields = new[]
    {
        "name", "clan_level", "member_num", "motto", "emblem_mark_type",
        "emblem_base_type", "emblem_main_color", "emblem_sub_color", "master_id",
        "created"
    };

    private static readonly string[] ClanMembershipFields = new[]
    {
        "character_id", "clan_id", "rank", "permission", "created"
    };

    private static readonly string[] ClanShopFields = new[]
    {
        "clan_id", "lineup_id"
    };

    private static readonly string[] ClanBaseCustomizationFields = new[]
    {
        "clan_id", "type", "furniture_id"
    };

    private readonly string SqlCDataClanMemberInfo =
        "SELECT \"ddon_clan_membership\".\"character_id\", \"ddon_clan_membership\".\"rank\", \"ddon_clan_membership\".\"permission\", \"ddon_clan_membership\".\"created\", \"ddon_character\".\"first_name\", \"ddon_character\".\"last_name\", \"ddon_character_job_data\".\"job\", \"ddon_character_job_data\".\"lv\" "
        + "FROM \"ddon_clan_membership\" "
        + "INNER JOIN \"ddon_character\" ON \"ddon_clan_membership\".\"character_id\" = \"ddon_character\".\"character_id\" AND \"ddon_character\".\"character_id\" = @character_id "
        + "INNER JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
        + "INNER JOIN \"ddon_character_job_data\" ON \"ddon_character_job_data\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" AND \"ddon_character_job_data\".\"job\" = \"ddon_character_common\".\"job\";";

    private readonly string SqlCDataClanMemberInfoList =
        "SELECT \"ddon_clan_membership\".\"character_id\", \"ddon_clan_membership\".\"rank\", \"ddon_clan_membership\".\"permission\", \"ddon_clan_membership\".\"created\", \"ddon_character\".\"first_name\", \"ddon_character\".\"last_name\", \"ddon_character_job_data\".\"job\", \"ddon_character_job_data\".\"lv\" "
        + "FROM \"ddon_clan_membership\" "
        + "INNER JOIN \"ddon_character\" ON \"ddon_clan_membership\".\"character_id\" = \"ddon_character\".\"character_id\" AND \"ddon_clan_membership\".\"clan_id\" = @clan_id "
        + "INNER JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
        + "INNER JOIN \"ddon_character_job_data\" ON \"ddon_character_job_data\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" AND \"ddon_character_job_data\".\"job\" = \"ddon_character_common\".\"job\";";

    private readonly string SqlDeleteClanBaseCustomization = "DELETE FROM \"ddon_clan_base_customization\" WHERE \"clan_id\" = @clan_id AND \"type\" = @type;";

    private readonly string SqlDeleteClanMembership = "DELETE FROM \"ddon_clan_membership\" WHERE \"character_id\"=@character_id;";
    private readonly string SqlDeleteClanParam = "DELETE FROM \"ddon_clan_param\" WHERE \"clan_id\"=@clan_id;";

    private readonly string SqlIncrementClanMemberNum = "UPDATE \"ddon_clan_param\" SET \"member_num\" = \"member_num\" + @value WHERE \"clan_id\" = @clan_id;";

    private readonly string SqlInsertClanBaseCustomization =
        $"INSERT INTO \"ddon_clan_base_customization\" ({BuildQueryField(ClanBaseCustomizationFields)}) VALUES ({BuildQueryInsert(ClanBaseCustomizationFields)});";

    private readonly string SqlInsertClanMembership =
        $"INSERT INTO \"ddon_clan_membership\" ({BuildQueryField(ClanMembershipFields)}) VALUES ({BuildQueryInsert(ClanMembershipFields)});";

    private readonly string SqlInsertClanParam = $"INSERT INTO \"ddon_clan_param\" ({BuildQueryField(ClanParamFields)}) VALUES ({BuildQueryInsert(ClanParamFields)});";

    private readonly string SqlInsertClanShopPurchase =
        $"INSERT INTO \"ddon_clan_shop_purchases\" ({BuildQueryField(ClanShopFields)}) VALUES ({BuildQueryInsert(ClanShopFields)});";

    private readonly string SqlSelectClanBaseCustomizations =
        $"SELECT {BuildQueryField(ClanBaseCustomizationFields)} FROM \"ddon_clan_base_customization\" WHERE \"clan_id\" = @clan_id;";

    private readonly string SqlSelectClanMembershipByCharacterId =
        $"SELECT {BuildQueryField(ClanMembershipFields)} FROM \"ddon_clan_membership\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlSelectClanParamById = $"SELECT \"clan_id\", {BuildQueryField(ClanParamFields)} FROM \"ddon_clan_param\" WHERE \"clan_id\"=@clan_id;";

    private readonly string SqlSelectClanBySearch =
        $"SELECT \"clan_id\", {BuildQueryField(ClanSearchResultFields)} FROM \"ddon_clan_param\" "
        + "WHERE (@dont_filter_by_name OR name LIKE @name) "
        + "AND (@dont_filter_by_clan_level OR clan_level >= @clan_level) "
        + "AND (@dont_filter_by_member_num OR member_num >= @member_num) "
        + "AND (@dont_filter_by_motto OR motto & @motto > 0) "
        + "AND (@dont_filter_by_active_days OR active_days & @active_days > 0) "
        + "AND (@dont_filter_by_active_time OR active_time & @active_time > 0) "
        + "AND (@dont_filter_by_characteristic OR characteristic & @characteristic > 0) "
        + "LIMIT 256;"; // limit imposed by client

    private readonly string SqlSelectClanShopPurchases = $"SELECT {BuildQueryField(ClanShopFields)} FROM \"ddon_clan_shop_purchases\" WHERE \"clan_id\" = @clan_id;";

    private readonly string SqlUpdateClanBaseCustomization =
        $"UPDATE \"ddon_clan_base_customization\" SET {BuildQueryUpdate(ClanBaseCustomizationFields)} WHERE \"clan_id\" = @clan_id AND \"type\" = @type;";

    private readonly string SqlUpdateClanMembership = $"UPDATE \"ddon_clan_membership\" SET {BuildQueryUpdate(ClanMembershipFields)} WHERE \"character_id\" = @character_id";
    private readonly string SqlUpdateClanParam = $"UPDATE \"ddon_clan_param\" SET {BuildQueryUpdate(ClanParamFields)} WHERE \"clan_id\" = @clan_id;";

    public override bool CreateClan(CDataClanParam clanParam)
    {
        return ExecuteInTransaction(conn =>
        {
            ExecuteNonQuery(
                conn,
                SqlInsertClanParam,
                command => { AddParameter(command, clanParam); },
                out long clanId
            );
            clanParam.ClanServerParam.ID = (uint)clanId;

            InsertClanMember(clanParam.ClanServerParam.MasterInfo, clanParam.ClanServerParam.ID, conn);
        });
    }

    public override CDataClanParam SelectClan(uint clanId, DbConnection? connectionIn = null)
    {
        CDataClanParam clanParam = new();

        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            uint masterId = 0;
            ExecuteReader(
                connection,
                SqlSelectClanParamById,
                command => { AddParameter(command, "@clan_id", clanId); },
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
                CDataClanMemberInfo master = GetClanMember(masterId);
                if (master is not null)
                    clanParam.ClanServerParam.MasterInfo = master;
                else
                    ExecuteNonQuery(
                        connection,
                        "UPDATE \"ddon_clan_param\" SET \"master_id\" = 0 WHERE \"clan_id\" = @clan_id;",
                        command => { AddParameter(command, "@clan_id", clanId); }
                    );
            }
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }

        return clanParam;
    }

    public override List<CDataClanSearchResult> SearchClans(CDataClanSearchParam search, DbConnection? connectionIn = null)
    {
        List<CDataClanSearchResult> list = new();

        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(
                connection,
                SqlSelectClanBySearch,
                command =>
                {
                    AddParameter(command, "@dont_filter_by_name", search.Name == "");
                    AddParameter(command, "@dont_filter_by_clan_level", search.Level == 0);
                    AddParameter(command, "@dont_filter_by_member_num", search.MemberNum == 0);
                    AddParameter(command, "@dont_filter_by_motto", search.Motto == 0);
                    AddParameter(command, "@dont_filter_by_active_days", search.ActiveDays == 0);
                    AddParameter(command, "@dont_filter_by_active_time", search.ActiveTime == 0);
                    AddParameter(command, "@dont_filter_by_characteristic", search.Characteristic == 0);

                    AddParameter(command, "@name", $"%{search.Name}%");
                    AddParameter(command, "@clan_level", search.Level);
                    AddParameter(command, "@member_num", search.MemberNum);
                    AddParameter(command, "@motto", search.Motto);
                    AddParameter(command, "@active_days", search.ActiveDays);
                    AddParameter(command, "@active_time", search.ActiveTime);
                    AddParameter(command, "@characteristic", search.Characteristic);
                },
                reader =>
                {
                    while (reader.Read())
                    {
                        CDataClanSearchResult result = ReadClanSearchResult(reader);
                        CDataClanMemberInfo master = GetClanMember(result.MasterCharacterId);
                        if (master is not null)
                            result.MasterCharacterName = master.CharacterListElement.CommunityCharacterBaseInfo.CharacterName;

                        list.Add(result);
                    }
                });
        });

        return list;
    }

    public override uint SelectClanMembershipByCharacterId(uint characterId, DbConnection? connectionIn = null)
    {
        uint clanId = 0;
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            ExecuteReader(connection,
                SqlSelectClanMembershipByCharacterId,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    if (reader.Read()) clanId = GetUInt32(reader, "clan_id");
                });
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }

        return clanId;
    }

    public override List<CDataClanMemberInfo> GetClanMemberList(uint clanId, DbConnection? connectionIn = null)
    {
        List<CDataClanMemberInfo> list = new();
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            ExecuteReader(connection,
                SqlCDataClanMemberInfoList,
                command => { AddParameter(command, "@clan_id", clanId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        CDataClanMemberInfo info = ReadClanMemberInfo(reader);
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

    public override CDataClanMemberInfo GetClanMember(uint characterId, DbConnection? connectionIn = null)
    {
        CDataClanMemberInfo member = null;
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            ExecuteReader(connection,
                SqlCDataClanMemberInfo,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    if (reader.Read()) member = ReadClanMemberInfo(reader);
                });
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }

        return member;
    }

    public override ClanName GetClanNameByClanId(uint clanId, DbConnection? connectionIn = null)
    {
        ClanName clanName = new();

        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            ExecuteReader(
                connection,
                SqlSelectClanParamById,
                command => { AddParameter(command, "@clan_id", clanId); },
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

    public override bool UpdateClan(CDataClanParam clan, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlUpdateClanParam, command => { AddParameter(command, clan); }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public bool DeleteClan(uint clanId, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlDeleteClanParam, command => { AddParameter(command, "@clan_id", clanId); }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool DeleteClan(CDataClanParam clan, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlDeleteClanParam, command => { AddParameter(command, clan); }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public bool IncrementClanMemberNum(int value, uint clanId, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
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

    public override bool InsertClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            bool memberInsert = ExecuteNonQuery(
                connection,
                SqlInsertClanMembership,
                command => { AddParameter(command, memberInfo, clanId); }
            ) == 1;
            bool incrementNum = IncrementClanMemberNum(1, clanId, connection);
            return memberInsert && incrementNum;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool DeleteClanMember(uint characterId, uint clanId, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            bool memberInsert = ExecuteNonQuery(
                connection,
                SqlDeleteClanMembership,
                command => { AddParameter(command, "@character_id", characterId); }
            ) == 1;
            bool incrementNum = IncrementClanMemberNum(-1, clanId, connection);
            return memberInsert && incrementNum;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool UpdateClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            bool memberUpdate = ExecuteNonQuery(
                connection,
                SqlUpdateClanMembership,
                command => { AddParameter(command, memberInfo, clanId); }
            ) == 1;
            return memberUpdate;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override List<uint> SelectClanShopPurchases(uint clanId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<uint> list = new();
            ExecuteReader(connection,
                SqlSelectClanShopPurchases,
                command => { AddParameter(command, "@clan_id", clanId); },
                reader =>
                {
                    while (reader.Read()) list.Add(GetUInt32(reader, "lineup_id"));
                });

            return list;
        });
    }

    public override bool InsertClanShopPurchase(uint clanId, uint lineupId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(
                connection,
                SqlInsertClanShopPurchase,
                command =>
                {
                    AddParameter(command, "clan_id", clanId);
                    AddParameter(command, "lineup_id", lineupId);
                }
            ) == 1;
        });
    }

    public override List<(byte Type, uint Id)> SelectClanBaseCustomizations(uint clanId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<(byte Type, uint Id)> list = new();
            ExecuteReader(connection,
                SqlSelectClanBaseCustomizations,
                command => { AddParameter(command, "@clan_id", clanId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        (byte, uint) cust = (
                            GetByte(reader, "type"),
                            GetUInt32(reader, "furniture_id")
                        );
                        list.Add(cust);
                    }
                });

            return list;
        });
    }

    public override bool InsertOrUpdateClanBaseCustomization(uint clanId, byte type, uint furnitureId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            bool check = ExecuteNonQuery(
                connection,
                SqlUpdateClanBaseCustomization,
                command =>
                {
                    AddParameter(command, "clan_id", clanId);
                    AddParameter(command, "type", type);
                    AddParameter(command, "furniture_id", furnitureId);
                }
            ) == 1;

            if (!check)
                check = ExecuteNonQuery(
                    connection,
                    SqlInsertClanBaseCustomization,
                    command =>
                    {
                        AddParameter(command, "clan_id", clanId);
                        AddParameter(command, "type", type);
                        AddParameter(command, "furniture_id", furnitureId);
                    }
                ) == 1;

            return check;
        });
    }

    public override bool DeleteClanBaseCustomization(uint clanId, byte type, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(
                connection,
                SqlDeleteClanBaseCustomization,
                command =>
                {
                    AddParameter(command, "clan_id", clanId);
                    AddParameter(command, "type", type);
                }
            ) == 1;
        });
    }

    private void AddParameter(DbCommand command, CDataClanParam clanParam)
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
        AddParameter(command, "@created", clanParam.ClanUserParam.Created.UtcDateTime);
    }

    private void AddParameter(DbCommand command, CDataClanMemberInfo memberInfo, uint clanId)
    {
        AddParameter(command, "@character_id", memberInfo.CharacterListElement.CommunityCharacterBaseInfo.CharacterId);
        AddParameter(command, "@clan_id", clanId);
        AddParameter(command, "@rank", (uint)memberInfo.Rank);
        AddParameter(command, "@permission", memberInfo.Permission);
        AddParameter(command, "@created", memberInfo.Created.UtcDateTime);
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
            Created = GetDateTime(reader, "created")
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
            MoneyClanPoint = GetUInt32(reader, "money_clan_point")
        };

        return serverParam;
    }

    private CDataClanSearchResult ReadClanSearchResult(DbDataReader reader)
    {
        CDataClanSearchResult searchResult = new()
        {
            ClanId = GetUInt32(reader, "clan_id"),
            Name = GetString(reader, "name"),
            Level = GetUInt16(reader, "clan_level"),
            MemberNum = GetUInt16(reader, "member_num"),
            Motto = GetUInt32(reader, "motto"),
            EmblemMarkType = GetByte(reader, "emblem_mark_type"),
            EmblemBaseType = GetByte(reader, "emblem_base_type"),
            EmblemMainColor = GetByte(reader, "emblem_main_color"),
            EmblemSubColor = GetByte(reader, "emblem_sub_color"),
            MasterCharacterId = GetUInt32(reader, "master_id"),
            Created = GetDateTime(reader, "created")
        };

        return searchResult;
    }

    private CDataClanMemberInfo ReadClanMemberInfo(DbDataReader reader)
    {
        CDataClanMemberInfo member = new()
        {
            Rank = (ClanMemberRank)GetUInt32(reader, "rank"),
            Permission = GetUInt32(reader, "permission"),
            Created = GetDateTime(reader, "created"),
            CharacterListElement = new CDataCharacterListElement
            {
                CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo
                {
                    CharacterId = GetUInt32(reader, "character_id"),
                    CharacterName = new CDataCharacterName
                    {
                        FirstName = GetString(reader, "first_name"),
                        LastName = GetString(reader, "last_name")
                    }
                },
                CurrentJobBaseInfo = new CDataJobBaseInfo
                {
                    Job = (JobId)GetByte(reader, "job"),
                    Level = GetByte(reader, "lv")
                }
            }
        };
        return member;
    }
}
