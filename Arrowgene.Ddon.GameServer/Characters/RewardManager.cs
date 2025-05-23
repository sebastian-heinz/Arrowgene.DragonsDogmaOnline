using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class RewardManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RewardManager));

        private readonly DdonGameServer _Server;
        public RewardManager(DdonGameServer server)
        {
            _Server = server;
        }

        public bool AddQuestRewards(GameClient client, Quest quest, DbConnection? connectionIn = null)
        {
            var rewards = quest.GenerateBoxRewards();

            var currentRewards = GetQuestBoxRewards(client, connectionIn);
            if (currentRewards.Count >= _Server.GameSettings.GameServerSettings.RewardBoxMax)
            {
                return false;
            }

            return _Server.Database.InsertBoxRewardItems(client.Character.CommonId, rewards, connectionIn);
        }

        public List<QuestBoxRewards> GetQuestBoxRewards(GameClient client, DbConnection? connectionIn = null)
        {
            return _Server.Database.SelectBoxRewardItems(client.Character.CommonId, connectionIn);
        }

        public bool DeleteQuestBoxReward(GameClient client, uint uniqId, DbConnection? connectionIn = null)
        {
            return _Server.Database.DeleteBoxRewardItem(client.Character.CommonId, uniqId, connectionIn);
        }
    }
}
