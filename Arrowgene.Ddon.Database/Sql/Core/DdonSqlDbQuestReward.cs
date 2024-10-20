using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {

        private readonly int MAX_RANDOM_REWARDS = 4;
        protected static readonly string[] RewardBoxFields = new string[]
        {
            /* uniq_reward_id */  "character_common_id", "quest_schedule_id", "num_random_rewards", "random_reward0_index", "random_reward1_index", "random_reward2_index", "random_reward3_index"
        };

        private readonly string SqlInsertRewardBoxItems = $"INSERT INTO \"ddon_reward_box\" ({BuildQueryField(RewardBoxFields)}) VALUES ({BuildQueryInsert(RewardBoxFields)});";
        private readonly string SqlSelectRewardBoxItems = $"SELECT \"uniq_reward_id\", {BuildQueryField(RewardBoxFields)} FROM \"ddon_reward_box\" WHERE \"character_common_id\" = @character_common_id;";
        private readonly string SqlDeleteRewardBoxItem = $"DELETE FROM \"ddon_reward_box\" WHERE \"uniq_reward_id\"=@uniq_reward_id AND \"character_common_id\"=@character_common_id;";

        public bool InsertBoxRewardItems(uint commonId, QuestBoxRewards rewards)
        {
            using TCon connection = OpenNewConnection();
            return InsertBoxRewardItems(connection, commonId, rewards);
        }

        public bool InsertBoxRewardItems(TCon conn, uint commonId, QuestBoxRewards rewards)
        {
            return ExecuteNonQuery(conn, SqlInsertRewardBoxItems, command =>
            {
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "quest_schedule_id", rewards.QuestScheduleId);
                AddParameter(command, "num_random_rewards", rewards.NumRandomRewards);

                int i;
                for(i = 0; i < rewards.NumRandomRewards; i++)
                {
                    AddParameter(command, $"random_reward{i}_index", rewards.RandomRewardIndices[i]);
                }

                for (; i < MAX_RANDOM_REWARDS; i++)
                {
                    AddParameter(command, $"random_reward{i}_index", 0);
                }
            }, out long autoIncrement) == 1;
        }

        public List<QuestBoxRewards> SelectBoxRewardItems(uint commonId)
        {
            using TCon connection = OpenNewConnection();
            return SelectBoxRewardItems(connection, commonId);
        }

        public List<QuestBoxRewards> SelectBoxRewardItems(TCon conn, uint commonId)
        {
            List<QuestBoxRewards> results = new List<QuestBoxRewards>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectRewardBoxItems,
                    command => {
                        AddParameter(command, "@character_common_id", commonId);
                    }, reader => {
                        while (reader.Read())
                        {
                            var result = ReadDatabaseQuestBoxReward(reader);
                            results.Add(result);
                        }
                    });
            });

            return results;
        }

        public bool DeleteBoxRewardItem(uint commonId, uint uniqId)
        {
            using TCon connection = OpenNewConnection();
            return DeleteBoxRewardItem(connection, commonId, uniqId);
        }

        public bool DeleteBoxRewardItem(TCon conn, uint commonId, uint uniqId)
        {
            return ExecuteNonQuery(conn, SqlDeleteRewardBoxItem, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@uniq_reward_id", uniqId);
            }) == 1;
        }

        private QuestBoxRewards ReadDatabaseQuestBoxReward(TReader reader)
        {
            QuestBoxRewards obj = new QuestBoxRewards();
            obj.UniqRewardId = GetUInt32(reader, "uniq_reward_id");
            obj.CharacterCommonId = GetUInt32(reader, "character_common_id");
            obj.NumRandomRewards = GetInt32(reader, "num_random_rewards");
            obj.QuestScheduleId = GetUInt32(reader, "quest_schedule_id");

            for (int i = 0; i < obj.NumRandomRewards; i++)
            {
                obj.RandomRewardIndices.Add(GetInt32(reader, $"random_reward{i}_index"));
            }

            return obj;
        }
    }
}

