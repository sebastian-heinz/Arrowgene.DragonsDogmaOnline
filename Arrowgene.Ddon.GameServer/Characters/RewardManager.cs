using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class RewardManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RewardManager));

        private readonly DdonGameServer Server;
        public RewardManager(DdonGameServer server)
        {
            Server = server;
        }

        public bool AddQuestRewards(GameClient client, Quest quest, DbConnection? connectionIn = null)
        {
            var rewards = quest.GenerateBoxRewards();
            return AddQuestRewards(client, rewards, connectionIn);
        }

        public bool AddQuestRewards(GameClient client, Quest quest, QuestBoxRewardFlags rewardFlags, DbConnection? connectionIn = null)
        {
            var rewards = quest.GenerateBoxRewards(rewardFlags);
            return AddQuestRewards(client, rewards, connectionIn);
        }

        private bool AddQuestRewards(GameClient client, QuestBoxRewards rewards, DbConnection? connectionIn = null)
        {
            var currentRewards = GetQuestBoxRewards(client, connectionIn);
            if (currentRewards.Count >= Server.GameSettings.GameServerSettings.RewardBoxMax)
            {
                return false;
            }

            return Server.Database.InsertBoxRewardItems(client.Character.CommonId, rewards, connectionIn);
        }

        public bool AddRepeatClearQuestRewards(GameClient client, Quest quest, DbConnection? connectionIn = null)
        {
            var rewards = quest.GenerateRepeatClearBoxRewards();

            return AddQuestRewards(client, rewards, connectionIn);
        }

        public bool AddAutoRepeatClearQuestRewards(GameClient client, Quest quest, DbConnection? connectionIn = null)
        {
            var rewards = quest.GenerateAutoRepeatClearBoxRewards();
            if (rewards.NumRandomRewards == 0)
            {
                return true;
            }

            return AddQuestRewards(client, rewards, connectionIn);
        }

        public List<QuestBoxRewards> GetQuestBoxRewards(GameClient client, DbConnection? connectionIn = null)
        {
            var rewards = Server.Database.SelectBoxRewardItems(client.Character.CommonId, connectionIn);
            MaterializeLegacyRewardBoxItems(rewards, connectionIn);
            return rewards;
        }

        public bool DeleteQuestBoxReward(GameClient client, uint uniqId, DbConnection? connectionIn = null)
        {
            return Server.Database.DeleteBoxRewardItem(client.Character.CommonId, uniqId, connectionIn);
        }

        private void MaterializeLegacyRewardBoxItems(List<QuestBoxRewards> boxRewards, DbConnection? connectionIn = null)
        {
            foreach (var boxReward in boxRewards)
            {
                if (boxReward.RewardItemList.Count > 0)
                {
                    continue;
                }

                var rewardItems = Quest.AsCDataRewardBoxItems(boxReward);
                if (rewardItems.Count == 0)
                {
                    continue;
                }

                boxReward.RewardItemList = rewardItems;
                foreach (var rewardItem in rewardItems)
                {
                    Server.Database.InsertBoxRewardItem(boxReward.UniqRewardId, rewardItem, connectionIn);
                }
            }
        }

        public PacketQueue UnlockEM4Skills(GameClient client, DbConnection? connectionIn = null)
        {
            var packets = new PacketQueue();

            var jobId = client.Character.Job;
            if (!SkillData.Em4CustomSkills.ContainsKey(jobId))
            {
                return new();
            }

            var releaseId = SkillData.Em4CustomSkills[jobId];
            if (client.Character.LearnedCustomSkills.Where(x => x.Job == jobId && x.SkillId == releaseId).Any())
            {
                return new();
            }

            Server.CharacterManager.UnlockCustomSkill(client.Character, jobId, releaseId, 1);
            Server.JobManager.UnlockCustomSkill(client, client.Character, jobId, releaseId, 1, connectionIn);

            var unlockedSkill = new S2CSkillAcquirementLearnNtc()
            {
                SkillParamList = [
                    new()
                    {
                        Job = jobId,
                        SkillNo = releaseId,
                        SkillLv = 1,
                    }
                ]
            };
            packets.Enqueue(client, unlockedSkill);

            return packets;
        }
    }
}
