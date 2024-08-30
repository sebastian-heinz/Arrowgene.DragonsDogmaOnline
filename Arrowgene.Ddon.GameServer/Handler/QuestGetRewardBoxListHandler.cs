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
                res.RewardBoxRecordList.Add(new CDataRewardBoxRecord()
                {
                    ListNo = listNo,
                    QuestId = (uint)boxReward.QuestId,
                    RewardItemList = Quest.AsCDataRewardBoxItems(boxReward),
                    VariantId = boxReward.VariantId
                });

                listNo += 1;
            }

            return res;
        }
    }
}
