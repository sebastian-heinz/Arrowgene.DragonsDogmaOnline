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
    public class QuestGetRewardBoxListHandler : GameRequestPacketHandler<C2SQuestGetRewardBoxListReq, S2CQuestGetRewardBoxListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetRewardBoxListHandler));

        public QuestGetRewardBoxListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetRewardBoxListRes Handle(GameClient client, C2SQuestGetRewardBoxListReq packet)
        {
            // client.Send(GameFull.Dump_901);
            S2CQuestGetRewardBoxListRes res = new S2CQuestGetRewardBoxListRes();

            uint listNo = 1;
            foreach (var boxReward in Server.RewardManager.GetQuestBoxRewards(client))
            {
                var quest = QuestManager.GetQuestByScheduleId(boxReward.QuestScheduleId);
                if (quest == null)
                {
                    listNo += 1;
                    Logger.Error($"Quest reward for QuestScheduleId={boxReward.QuestScheduleId}, but no definition of quest exists.");
                    continue;
                }

                res.RewardBoxRecordList.Add(new CDataRewardBoxRecord()
                {
                    ListNo = listNo,
                    QuestId = (uint)quest.QuestId,
                    RewardItemList = Quest.AsCDataRewardBoxItems(boxReward)
                });

                listNo += 1;
            }

            return res;
        }
    }
}
