using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] BitterBlackMazeProgressFields =
    [
        "character_id", "start_time", "content_id", "content_mode", "tier", "killed_death", "last_ticket_time"
    ];

    private readonly string SqlDeleteBBMProgress = "DELETE FROM \"ddon_bbm_progress\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertBBMProgress =
        $"INSERT INTO \"ddon_bbm_progress\" ({BuildQueryField(BitterBlackMazeProgressFields)}) VALUES ({BuildQueryInsert(BitterBlackMazeProgressFields)});";

    private readonly string SqlSelectBBMProgress = $"SELECT {BuildQueryField(BitterBlackMazeProgressFields)} FROM \"ddon_bbm_progress\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlUpdateBBMProgress = $"UPDATE \"ddon_bbm_progress\" SET {BuildQueryUpdate(BitterBlackMazeProgressFields)} WHERE \"character_id\"=@character_id;";

    public override bool InsertBBMProgress(uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath, ulong lastTicketTime)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertBBMProgress(connection, characterId, startTime, contentId, contentMode, tier, killedDeath, lastTicketTime);
    }

    public bool InsertBBMProgress(DbConnection connection, uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath,
        ulong lastTicketTime)
    {
        return ExecuteNonQuery(connection, SqlInsertBBMProgress, command =>
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "start_time", startTime);
            AddParameter(command, "content_id", contentId);
            AddParameter(command, "content_mode", (uint)contentMode);
            AddParameter(command, "tier", tier);
            AddParameter(command, "killed_death", killedDeath);
            AddParameter(command, "last_ticket_time", lastTicketTime);
        }) == 1;
    }

    public override bool UpdateBBMProgress(uint characterId, BitterblackMazeProgress progress, DbConnection? connectionIn = null)
    {
        return UpdateBBMProgress(characterId, progress.StartTime, progress.ContentId, progress.ContentMode, progress.Tier, progress.KilledDeath, progress.LastTicketTime,
            connectionIn);
    }

    public bool UpdateBBMProgress(uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath, ulong lastTicketTime,
        DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateBBMProgress, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "start_time", startTime);
                AddParameter(command, "content_id", contentId);
                AddParameter(command, "content_mode", (uint)contentMode);
                AddParameter(command, "tier", tier);
                AddParameter(command, "killed_death", killedDeath);
                AddParameter(command, "last_ticket_time", lastTicketTime);
            }) == 1;
        });
    }

    public override bool RemoveBBMProgress(uint characterId)
    {
        using DbConnection connection = OpenNewConnection();
        return RemoveBBMProgress(connection, characterId);
    }

    public bool RemoveBBMProgress(DbConnection connection, uint characterId)
    {
        return ExecuteNonQuery(connection, SqlDeleteBBMProgress, command => { AddParameter(command, "character_id", characterId); }) == 1;
    }

    public override BitterblackMazeProgress SelectBBMProgress(uint characterId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectBBMProgress(connection, characterId);
    }

    public BitterblackMazeProgress SelectBBMProgress(DbConnection connection, uint characterId)
    {
        BitterblackMazeProgress result = null;
        ExecuteInTransaction(connection =>
        {
            ExecuteReader(connection, SqlSelectBBMProgress, command => { AddParameter(command, "character_id", characterId); }, reader =>
            {
                if (reader.Read())
                {
                    result = new BitterblackMazeProgress();
                    result.StartTime = GetUInt64(reader, "start_time");
                    result.ContentId = GetUInt32(reader, "content_id");
                    result.ContentMode = (BattleContentMode)GetUInt32(reader, "content_mode");
                    result.Tier = GetUInt32(reader, "tier");
                    result.KilledDeath = GetBoolean(reader, "killed_death");
                    result.LastTicketTime = GetUInt64(reader, "last_ticket_time");
                }
            });
        });

        return result;
    }
}
