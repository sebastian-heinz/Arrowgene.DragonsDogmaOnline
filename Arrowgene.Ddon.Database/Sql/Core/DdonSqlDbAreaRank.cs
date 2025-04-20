using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string[] DdonAreaRankFields =
    [
        "character_id", "area_id", "rank", "point", "week_point", "last_week_point"
    ];

    private static readonly string[] DdonAreaRankSupplyFields =
    [
        "character_id", "area_id", "index", "item_id", "num"
    ];

    private readonly string SqlDeleteAreaRankSupplyAll = "DELETE FROM \"ddon_area_rank_supply\"";

    private readonly string SqlDeleteAreaRankSupplySingle =
        "DELETE FROM \"ddon_area_rank_supply\" WHERE \"character_id\"=@character_id AND \"area_id\"=@area_id AND \"index\"=@index;";

    private readonly string SqlInsertAreaRank = $"INSERT INTO \"ddon_area_rank\" ({BuildQueryField(DdonAreaRankFields)}) VALUES ({BuildQueryInsert(DdonAreaRankFields)});";

    private readonly string SqlInsertAreaRankSupply =
        $"INSERT INTO \"ddon_area_rank_supply\" ({BuildQueryField(DdonAreaRankSupplyFields)}) VALUES ({BuildQueryInsert(DdonAreaRankSupplyFields)});";

    private readonly string SqlResetAreaRankPoint = "UPDATE \"ddon_area_rank\" SET \"last_week_point\" = \"week_point\", \"week_point\" = 0;";

    private readonly string SqlSelectAreaRank = $"SELECT {BuildQueryField(DdonAreaRankFields)} FROM \"ddon_area_rank\" WHERE \"character_id\"=@character_id;";
    private readonly string SqlSelectAreaRankAll = $"SELECT {BuildQueryField(DdonAreaRankFields)} FROM \"ddon_area_rank\" WHERE \"rank\">0;";

    private readonly string SqlSelectAreaRankSupply =
        $"SELECT {BuildQueryField(DdonAreaRankSupplyFields)} FROM \"ddon_area_rank_supply\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlSelectAreaRankSupplySingle =
        $"SELECT {BuildQueryField(DdonAreaRankSupplyFields)} FROM \"ddon_area_rank_supply\" WHERE \"character_id\"=@character_id AND \"area_id\"=@area_id;";

    private readonly string SqlUpdateAreaRank =
        $"UPDATE \"ddon_area_rank\" SET {BuildQueryUpdate(DdonAreaRankFields)} WHERE \"character_id\"=@character_id AND \"area_id\"=@area_id;";

    private readonly string SqlUpdateAreaRankSupply =
        $"UPDATE \"ddon_area_rank_supply\" SET {BuildQueryUpdate(DdonAreaRankSupplyFields)} WHERE \"character_id\"=@character_id AND \"area_id\"=@area_id AND \"index\"=@index;";

    public override bool InsertAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null)
    {
        Logger.Debug("Inserting area rank.");
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertAreaRank, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@area_id", (uint)areaRank.AreaId);
                AddParameter(command, "@rank", areaRank.Rank);
                AddParameter(command, "@point", areaRank.Point);
                AddParameter(command, "@week_point", areaRank.WeekPoint);
                AddParameter(command, "@last_week_point", areaRank.LastWeekPoint);
            }) == 1;
        });
    }

    public override bool UpdateAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateAreaRank, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@area_id", (uint)areaRank.AreaId);
                AddParameter(command, areaRank);
            }) == 1;
        });
    }

    public override Dictionary<QuestAreaId, AreaRank> SelectAreaRank(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            Dictionary<QuestAreaId, AreaRank> list = new();
            ExecuteReader(connection,
                SqlSelectAreaRank,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        AreaRank rank = new()
                        {
                            AreaId = (QuestAreaId)GetUInt32(reader, "area_id"),
                            Rank = GetUInt32(reader, "rank"),
                            Point = GetUInt32(reader, "point"),
                            WeekPoint = GetUInt32(reader, "week_point"),
                            LastWeekPoint = GetUInt32(reader, "last_week_point")
                        };
                        list[rank.AreaId] = rank;
                    }
                });
            return list;
        });
    }

    public override List<(uint CharacterId, AreaRank Rank)> SelectAllAreaRank(DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<(uint CharacterId, AreaRank Rank)> list = new();
            ExecuteReader(connection,
                SqlSelectAreaRankAll,
                command => { },
                reader =>
                {
                    while (reader.Read())
                    {
                        uint characterId = GetUInt32(reader, "character_id");
                        AreaRank rank = new()
                        {
                            AreaId = (QuestAreaId)GetUInt32(reader, "area_id"),
                            Rank = GetUInt32(reader, "rank"),
                            Point = GetUInt32(reader, "point"),
                            WeekPoint = GetUInt32(reader, "week_point"),
                            LastWeekPoint = GetUInt32(reader, "last_week_point")
                        };
                        list.Add((characterId, rank));
                    }
                });
            return list;
        });
    }

    public override bool ResetAreaRankPoint(DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection => { return ExecuteNonQuery(connection, SqlResetAreaRankPoint, command => { }) > 0; });
    }

    public override bool InsertAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertAreaRankSupply, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@area_id", (uint)areaId);
                AddParameter(command, "@index", index);
                AddParameter(command, "@item_id", itemId);
                AddParameter(command, "@num", num);
            }) == 1;
        });
    }

    public override bool UpdateAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            if (num == 0)
                return ExecuteNonQuery(connection, SqlDeleteAreaRankSupplySingle, command =>
                {
                    AddParameter(command, "@character_id", characterId);
                    AddParameter(command, "@area_id", (uint)areaId);
                    AddParameter(command, "@index", index);
                    AddParameter(command, "@item_id", itemId);
                    AddParameter(command, "@num", num);
                }) == 1;

            return ExecuteNonQuery(connection, SqlUpdateAreaRankSupply, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@area_id", (uint)areaId);
                AddParameter(command, "@index", index);
                AddParameter(command, "@item_id", itemId);
                AddParameter(command, "@num", num);
            }) == 1;
        });
    }

    public override Dictionary<QuestAreaId, List<CDataRewardItemInfo>> SelectAreaRankSupply(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            Dictionary<QuestAreaId, List<CDataRewardItemInfo>> ret = new();
            ExecuteReader(connection,
                SqlSelectAreaRankSupply,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        QuestAreaId areaId = (QuestAreaId)GetUInt32(reader, "area_id");
                        CDataRewardItemInfo reward = new()
                        {
                            Index = GetUInt32(reader, "index"),
                            ItemId = GetUInt32(reader, "item_id"),
                            Num = GetByte(reader, "num")
                        };
                        if (!ret.ContainsKey(areaId)) ret[areaId] = new List<CDataRewardItemInfo>();

                        ret[areaId].Add(reward);
                    }
                });
            return ret;
        });
    }

    public override List<CDataRewardItemInfo> SelectAreaRankSupply(uint characterId, QuestAreaId areaId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<CDataRewardItemInfo> ret = new();
            ExecuteReader(connection,
                SqlSelectAreaRankSupplySingle,
                command =>
                {
                    AddParameter(command, "@character_id", characterId);
                    AddParameter(command, "@area_id", (uint)areaId);
                },
                reader =>
                {
                    while (reader.Read())
                    {
                        CDataRewardItemInfo reward = new()
                        {
                            Index = GetUInt32(reader, "index"),
                            ItemId = GetUInt32(reader, "item_id"),
                            Num = GetByte(reader, "num")
                        };
                        ret.Add(reward);
                    }
                });
            return ret;
        });
    }

    public override bool DeleteAreaRankSupply(DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection => { return ExecuteNonQuery(connection, SqlDeleteAreaRankSupplyAll, command => { }) > 0; });
    }

    private void AddParameter(DbCommand command, AreaRank areaRank)
    {
        AddParameter(command, "area_id", (uint)areaRank.AreaId);
        AddParameter(command, "rank", areaRank.Rank);
        AddParameter(command, "point", areaRank.Point);
        AddParameter(command, "week_point", areaRank.WeekPoint);
        AddParameter(command, "last_week_point", areaRank.LastWeekPoint);
    }
}
