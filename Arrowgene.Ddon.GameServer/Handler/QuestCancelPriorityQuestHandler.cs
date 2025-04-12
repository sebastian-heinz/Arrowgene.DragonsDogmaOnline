using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestCancelPriorityQuestHandler : GameRequestPacketHandler<C2SQuestCancelPriorityQuestReq, S2CQuestCancelPriorityQuestRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestCancelPriorityQuestHandler));

        public QuestCancelPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestCancelPriorityQuestRes Handle(GameClient client, C2SQuestCancelPriorityQuestReq packet)
        {
            Server.Database.DeletePriorityQuest(client.Character.CommonId, packet.QuestScheduleId);

            client.Party.QuestState.UpdatePriorityQuestList(client).Send();

            return new S2CQuestCancelPriorityQuestRes()
            {
                QuestScheduleId = packet.QuestScheduleId
            };
        }
    }
}
