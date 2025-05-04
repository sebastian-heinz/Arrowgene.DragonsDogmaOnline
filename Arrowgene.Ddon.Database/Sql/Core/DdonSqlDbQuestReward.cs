using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] RewardBoxFields = new[]
    {
        /* uniq_reward_id */ "character_common_id", "quest_schedule_id", "num_random_rewards", "random_reward0_index", "random_reward1_index", "random_reward2_index",
        "random_reward3_index"
    };

    private readonly int MAX_RANDOM_REWARDS = 4;

    private readonly string SqlDeleteRewardBoxItem =
        "DELETE FROM \"ddon_reward_box\" WHERE \"uniq_reward_id\"=@uniq_reward_id AND \"character_common_id\"=@character_common_id;";

    private readonly string SqlInsertRewardBoxItems = $"INSERT INTO \"ddon_reward_box\" ({BuildQueryField(RewardBoxFields)}) VALUES ({BuildQueryInsert(RewardBoxFields)});";

    private readonly string SqlSelectRewardBoxItems =
        $"SELECT \"uniq_reward_id\", {BuildQueryField(RewardBoxFields)} FROM \"ddon_reward_box\" WHERE \"character_common_id\" = @character_common_id;";

    public override bool InsertBoxRewardItems(uint commonId, QuestBoxRewards rewards, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertRewardBoxItems, command =>
            {
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "quest_schedule_id", rewards.QuestScheduleId);
                AddParameter(command, "num_random_rewards", rewards.NumRandomRewards);

                int i;
                for (i = 0; i < rewards.NumRandomRewards; i++) AddParameter(command, $"random_reward{i}_index", rewards.RandomRewardIndices[i]);

                for (; i < MAX_RANDOM_REWARDS; i++) AddParameter(command, $"random_reward{i}_index", 0);
            }) == 1;
        });
    }

    public override List<QuestBoxRewards> SelectBoxRewardItems(uint commonId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<QuestBoxRewards> results = new();
            ExecuteReader(connection, SqlSelectRewardBoxItems,
                command => { AddParameter(command, "@character_common_id", commonId); }, reader =>
                {
                    while (reader.Read())
                    {
                        QuestBoxRewards result = ReadDatabaseQuestBoxReward(reader);
                        results.Add(result);
                    }
                });
            return results;
        });
    }

    public override bool DeleteBoxRewardItem(uint commonId, uint uniqId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeleteRewardBoxItem, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@uniq_reward_id", uniqId);
            }) == 1;
        });
    }

    private QuestBoxRewards ReadDatabaseQuestBoxReward(DbDataReader reader)
    {
        QuestBoxRewards obj = new();
        obj.UniqRewardId = GetUInt32(reader, "uniq_reward_id");
        obj.CharacterCommonId = GetUInt32(reader, "character_common_id");
        obj.NumRandomRewards = GetInt32(reader, "num_random_rewards");
        obj.QuestScheduleId = GetUInt32(reader, "quest_schedule_id");

        for (int i = 0; i < obj.NumRandomRewards; i++) obj.RandomRewardIndices.Add(GetInt32(reader, $"random_reward{i}_index"));

        return obj;
    }
}
