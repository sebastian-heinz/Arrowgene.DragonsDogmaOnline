using System.Data.Common;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] BitterBlackMazeRewardsFields = new[]
    {
        "character_id", "gold_marks", "silver_marks", "red_marks"
    };

    private readonly string SqlDeleteBBMRewards = "DELETE FROM \"ddon_bbm_rewards\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertBBMRewards =
        $"INSERT INTO \"ddon_bbm_rewards\" ({BuildQueryField(BitterBlackMazeRewardsFields)}) VALUES ({BuildQueryInsert(BitterBlackMazeRewardsFields)});";

    private readonly string SqlSelectBBMRewards = $"SELECT {BuildQueryField(BitterBlackMazeRewardsFields)} FROM \"ddon_bbm_rewards\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlUpdateBBMRewards = $"UPDATE \"ddon_bbm_rewards\" SET {BuildQueryUpdate(BitterBlackMazeRewardsFields)} WHERE \"character_id\"=@character_id;";

    public override bool InsertBBMRewards(uint characterId, uint goldMarks, uint silverMarks, uint redMarks)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertBBMRewards(connection, characterId, goldMarks, silverMarks, redMarks);
    }

    public bool InsertBBMRewards(DbConnection connection, uint characterId, uint goldMarks, uint silverMarks, uint redMarks)
    {
        return ExecuteNonQuery(connection, SqlInsertBBMRewards, command =>
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "gold_marks", goldMarks);
            AddParameter(command, "silver_marks", silverMarks);
            AddParameter(command, "red_marks", redMarks);
        }) == 1;
    }

    public override bool UpdateBBMRewards(uint characterId, BitterblackMazeRewards rewards, DbConnection? connectionIn = null)
    {
        return UpdateBBMRewards(characterId, rewards.GoldMarks, rewards.SilverMarks, rewards.RedMarks, connectionIn);
    }

    public bool UpdateBBMRewards(uint characterId, uint goldMarks, uint silverMarks, uint redMarks, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateBBMRewards, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "gold_marks", goldMarks);
                AddParameter(command, "silver_marks", silverMarks);
                AddParameter(command, "red_marks", redMarks);
            }) == 1;
        });
    }

    public override bool RemoveBBMRewards(uint characterId)
    {
        using DbConnection connection = OpenNewConnection();
        return RemoveBBMRewards(connection, characterId);
    }

    public bool RemoveBBMRewards(DbConnection connection, uint characterId)
    {
        return ExecuteNonQuery(connection, SqlDeleteBBMRewards, command => { AddParameter(command, "character_id", characterId); }) == 1;
    }

    public override BitterblackMazeRewards SelectBBMRewards(uint characterId, DbConnection? connectionIn = null)
    {
        BitterblackMazeRewards result = null;

        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectBBMRewards, command => { AddParameter(command, "character_id", characterId); }, reader =>
            {
                if (reader.Read())
                {
                    result = new BitterblackMazeRewards();
                    result.GoldMarks = GetUInt32(reader, "gold_marks");
                    result.SilverMarks = GetUInt32(reader, "silver_marks");
                    result.RedMarks = GetUInt32(reader, "red_marks");
                }
            });
        });

        return result;
    }
}
