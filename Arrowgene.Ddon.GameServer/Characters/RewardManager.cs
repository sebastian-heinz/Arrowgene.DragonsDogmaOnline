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

            var currentRewards = GetQuestBoxRewards(client, connectionIn);
            if (currentRewards.Count >= Server.GameSettings.GameServerSettings.RewardBoxMax)
            {
                return false;
            }

            return Server.Database.InsertBoxRewardItems(client.Character.CommonId, rewards, connectionIn);
        }

        public List<QuestBoxRewards> GetQuestBoxRewards(GameClient client, DbConnection? connectionIn = null)
        {
            return Server.Database.SelectBoxRewardItems(client.Character.CommonId, connectionIn);
        }

        public bool DeleteQuestBoxReward(GameClient client, uint uniqId, DbConnection? connectionIn = null)
        {
            return Server.Database.DeleteBoxRewardItem(client.Character.CommonId, uniqId, connectionIn);
        }

        public PacketQueue UnlockEM4Skills(GameClient client, DbConnection? connectionIn = null)
        {
            var packets = new PacketQueue();

            var unlockedSkills = new S2CSkillAcquirementLearnNtc();
            // TODO: Track quest completion on per job instead of unlocking all
            foreach (var (jobId, releaseId) in SkillData.Em4CustomSkills)
            {
                Server.CharacterManager.UnlockCustomSkill(client.Character, jobId, releaseId, 1);

                // Handle players who had existing skills before they were locked
                var existing = client.Character.LearnedCustomSkills.Where(x => x.SkillId == releaseId && x.Job == jobId).FirstOrDefault();
                if (existing == null)
                {
                    Server.JobManager.UnlockCustomSkill(client, client.Character, jobId, releaseId, 1, connectionIn);
                }

                unlockedSkills.SkillParamList.Add(new CDataSkillLevelBaseParam()
                {
                    Job = jobId,
                    SkillNo = releaseId,
                    SkillLv = existing?.SkillLv ?? 1,
                });
            }
            packets.Enqueue(client, unlockedSkills);

            return packets;
        }
    }
}
