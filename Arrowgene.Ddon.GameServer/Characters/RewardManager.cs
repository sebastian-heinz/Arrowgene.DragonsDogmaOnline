using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Logging;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class RewardManager
    {
        private readonly int MAX_REWARD_BOX_RESULTS = 100;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RewardManager));

        private readonly DdonGameServer _Server;
        public RewardManager(DdonGameServer server)
        {
            _Server = server;
        }

        public bool AddQuestRewards(GameClient client, Quest quest)
        {
            var rewards = quest.GenerateBoxRewards();

            var currentRewards = GetQuestBoxRewards(client);
            if (currentRewards.Count >= MAX_REWARD_BOX_RESULTS)
            {
                return false;
            }

            return _Server.Database.InsertBoxRewardItems(client.Character.CommonId, rewards);
        }

        public List<QuestBoxRewards> GetQuestBoxRewards(GameClient client)
        {
            return _Server.Database.SelectBoxRewardItems(client.Character.CommonId);
        }

        public bool DeleteQuestBoxReward(GameClient client, uint uniqId)
        {
            return _Server.Database.DeleteBoxRewardItem(client.Character.CommonId, uniqId);
        }
    }
}
