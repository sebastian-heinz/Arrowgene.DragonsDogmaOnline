using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteReleasedWarpPoint = "DELETE FROM \"ddon_released_warp_point\" WHERE \"character_id\"=@character_id AND \"warp_point_id\"=@warp_point_id";

    protected static readonly string[] ReleasedWarpPointFields = new[]
    {
        "character_id", "warp_point_id", "favorite_slot_no"
    };

    private static readonly string SqlUpdateReleasedWarpPoint =
        $"UPDATE \"ddon_released_warp_point\" SET {BuildQueryUpdate(ReleasedWarpPointFields)} WHERE \"character_id\"=@character_id AND \"warp_point_id\"=@warp_point_id";

    private static readonly string SqlSelectReleasedWarpPoints =
        $"SELECT {BuildQueryField(ReleasedWarpPointFields)} FROM \"ddon_released_warp_point\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertIfNotExistsReleasedWarpPoint =
        $"INSERT INTO \"ddon_released_warp_point\" ({BuildQueryField(ReleasedWarpPointFields)}) SELECT {BuildQueryInsert(ReleasedWarpPointFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_released_warp_point\" WHERE \"character_id\"=@character_id AND \"warp_point_id\"=@warp_point_id);";

    private readonly string SqlInsertReleasedWarpPoint =
        $"INSERT INTO \"ddon_released_warp_point\" ({BuildQueryField(ReleasedWarpPointFields)}) VALUES ({BuildQueryInsert(ReleasedWarpPointFields)});";

    public override List<ReleasedWarpPoint> SelectReleasedWarpPoints(uint characterId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectReleasedWarpPoints(connection, characterId);
    }

    public List<ReleasedWarpPoint> SelectReleasedWarpPoints(DbConnection connection, uint characterId)
    {
        List<ReleasedWarpPoint> rwps = new();
        ExecuteReader(connection, SqlSelectReleasedWarpPoints,
            command => { AddParameter(command, "@character_id", characterId); },
            reader =>
            {
                while (reader.Read()) rwps.Add(ReadReleasedWarpPoint(reader));
            });
        return rwps;
    }

    public override bool InsertIfNotExistsReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
    }

    public bool InsertIfNotExistsReleasedWarpPoint(DbConnection connection, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
    {
        return ExecuteNonQuery(connection, SqlInsertIfNotExistsReleasedWarpPoint, command => { AddParameter(command, characterId, ReleasedWarpPoint); }) == 1;
    }

    public override bool InsertIfNotExistsReleasedWarpPoints(uint characterId, List<ReleasedWarpPoint> releasedWarpPoints)
    {
        return ExecuteInTransaction(connection =>
        {
            foreach (ReleasedWarpPoint releasedWarpPoint in releasedWarpPoints) InsertIfNotExistsReleasedWarpPoint(connection, characterId, releasedWarpPoint);
        });
    }

    public override bool InsertReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
    }

    public bool InsertReleasedWarpPoint(DbConnection connection, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
    {
        return ExecuteNonQuery(connection, SqlInsertReleasedWarpPoint, command => { AddParameter(command, characterId, ReleasedWarpPoint); }) == 1;
    }

    public override bool ReplaceReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
    {
        using DbConnection connection = OpenNewConnection();
        return ReplaceReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
    }

    public bool ReplaceReleasedWarpPoint(DbConnection connection, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
    {
        Logger.Debug("Inserting wallet point.");
        if (!InsertIfNotExistsReleasedWarpPoint(connection, characterId, ReleasedWarpPoint))
        {
            Logger.Debug("Wallet point already exists, replacing.");
            return UpdateReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
        }

        return true;
    }

    public override bool UpdateReleasedWarpPoint(uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateReleasedWarpPoint(connection, characterId, updatedReleasedWarpPoint);
    }

    public bool UpdateReleasedWarpPoint(DbConnection connection, uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint)
    {
        return ExecuteNonQuery(connection, SqlUpdateReleasedWarpPoint, command => { AddParameter(command, characterId, updatedReleasedWarpPoint); }) == 1;
    }

    public override bool DeleteReleasedWarpPoint(uint characterId, uint warpPointId)
    {
        return ExecuteNonQuery(SqlDeleteReleasedWarpPoint, command =>
        {
            AddParameter(command, "@character_id", characterId);
            AddParameter(command, "@warp_point_id", (byte)warpPointId);
        }) == 1;
    }

    private ReleasedWarpPoint ReadReleasedWarpPoint(DbDataReader reader)
    {
        ReleasedWarpPoint ReleasedWarpPoint = new()
        {
            WarpPointId = GetUInt32(reader, "warp_point_id"),
            FavoriteSlotNo = GetUInt32(reader, "favorite_slot_no")
        };
        return ReleasedWarpPoint;
    }

    private void AddParameter(DbCommand command, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
    {
        AddParameter(command, "character_id", characterId);
        AddParameter(command, "warp_point_id", ReleasedWarpPoint.WarpPointId);
        AddParameter(command, "favorite_slot_no", ReleasedWarpPoint.FavoriteSlotNo);
    }
}
