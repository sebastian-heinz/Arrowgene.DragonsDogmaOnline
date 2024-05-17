using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetRewardBoxHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetRewardBoxHandler));


        public QuestGetRewardBoxHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_REWARD_BOX_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(GameFull.Dump_901);
            S2CQuestGetRewardBoxListRes res = new S2CQuestGetRewardBoxListRes();

            uint listNo = 1;
            foreach (var boxReward in client.Character.QuestRewards)
            {
                var quest = QuestManager.GetQuest(boxReward.QuestId);
                if (quest == null)
                {
                    // How did this happen?
                    continue;
                }

                res.RewardBoxRecordList.Add(new CDataRewardBoxRecord()
                {
                    ListNo = listNo,
                    QuestId = (uint) boxReward.QuestId,
                    RewardItemList = boxReward.Rewards.Values.ToList()
                });

                listNo += 1;
            }

            client.Send(res);
        }
    }
}
