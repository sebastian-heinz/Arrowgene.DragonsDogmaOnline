using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_epitaph_claimed_weekly_rewards */
    protected static readonly string[] EpitaphRoadClaimedWeeklyRewardsFields = new[]
    {
        "character_id", "epitaph_id"
    };

    private readonly string SqlDeleteWeeklyRewards = "DELETE FROM ddon_epitaph_claimed_weekly_rewards;";

    private readonly string SqlInsertEpitaphClaimedWeeklyRewards =
        $"INSERT INTO \"ddon_epitaph_claimed_weekly_rewards\" ({BuildQueryField(EpitaphRoadClaimedWeeklyRewardsFields)}) VALUES ({BuildQueryInsert(EpitaphRoadClaimedWeeklyRewardsFields)});";

    private readonly string SqlSelectEpitaphClaimedWeeklyRewards =
        $"SELECT {BuildQueryField(EpitaphRoadClaimedWeeklyRewardsFields)} FROM \"ddon_epitaph_claimed_weekly_rewards\" WHERE \"character_id\"=@character_id;";

    public override bool InsertEpitaphWeeklyReward(uint characterId, uint epitaphId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertEpitaphClaimedWeeklyRewards, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "epitaph_id", epitaphId);
            }) == 1;
        });
    }

    public override HashSet<uint> GetEpitaphClaimedWeeklyRewards(uint characterId, DbConnection? connectionIn = null)
    {
        HashSet<uint> results = new();
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectEpitaphClaimedWeeklyRewards, command => { AddParameter(command, "character_id", characterId); }, reader =>
            {
                while (reader.Read()) results.Add(GetUInt32(reader, "epitaph_id"));
            });
        });
        return results;
    }

    public override void DeleteWeeklyEpitaphClaimedRewards(DbConnection? connectionIn = null)
    {
        ExecuteQuerySafe(connectionIn, connection => { ExecuteNonQuery(connection, SqlDeleteWeeklyRewards, command => { }); });
    }
}
