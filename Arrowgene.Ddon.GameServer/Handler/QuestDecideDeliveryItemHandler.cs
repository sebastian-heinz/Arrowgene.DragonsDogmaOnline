using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestDecideDeliveryItemHandler : GameRequestPacketHandler<C2SQuestDecideDeliveryItemReq, S2CQuestDecideDeliveryItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestDecideDeliveryItemHandler));


        public QuestDecideDeliveryItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestDecideDeliveryItemRes Handle(GameClient client, C2SQuestDecideDeliveryItemReq request)
        {
            S2CQuestDecideDeliveryItemRes res = new S2CQuestDecideDeliveryItemRes()
            {
                QuestScheduleId = request.QuestScheduleId,
                ProcessNo = request.ProcessNo,
            };

            var quest = QuestManager.GetQuestByScheduleId(request.QuestScheduleId);
            var questStateManager = QuestManager.GetQuestStateManager(client, quest);
            var questState = questStateManager.GetQuestState(request.QuestScheduleId);
            if (questState.DeliveryRequestComplete(request.ProcessNo))
            {
                S2CQuestDecideDeliveryItemNtc ntc = new S2CQuestDecideDeliveryItemNtc()
                {
                    QuestScheduleId = request.QuestScheduleId,
                    ProcessNo = request.ProcessNo
                };

                if (quest.IsPersonal)
                {
                    client.Send(ntc);
                }
                else
                {
                    client.Party.SendToAll(ntc);
                }
            }

            return res;
        }
    }
}
