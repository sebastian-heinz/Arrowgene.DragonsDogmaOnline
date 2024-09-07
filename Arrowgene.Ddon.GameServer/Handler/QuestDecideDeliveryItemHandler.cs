using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestDecideDeliveryItemHandler : GameStructurePacketHandler<C2SQuestDecideDeliveryItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestDecideDeliveryItemHandler));


        public QuestDecideDeliveryItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestDecideDeliveryItemReq> packet)
        {
            S2CQuestDecideDeliveryItemRes res = new S2CQuestDecideDeliveryItemRes()
            {
                QuestScheduleId = packet.Structure.QuestScheduleId,
                ProcessNo = packet.Structure.ProcessNo,
            };

            var questState = client.Party.QuestState.GetQuestState(packet.Structure.QuestScheduleId);
            if (questState.DeliveryRequestComplete())
            {

                S2CQuestDecideDeliveryItemNtc ntc = new S2CQuestDecideDeliveryItemNtc()
                {
                    QuestScheduleId = packet.Structure.QuestScheduleId,
                    ProcessNo = packet.Structure.ProcessNo
                };

                client.Party.SendToAll(ntc);
            }

            client.Send(res);
        }
    }
}
