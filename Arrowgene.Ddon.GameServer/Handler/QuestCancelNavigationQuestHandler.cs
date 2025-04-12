using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestCancelNavigationQuestHandler : GameRequestPacketHandler<C2SQuestCancelNavigationQuestReq, S2CQuestCancelNavigationQuestRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestCancelNavigationQuestHandler));

        public QuestCancelNavigationQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestCancelNavigationQuestRes Handle(GameClient client, C2SQuestCancelNavigationQuestReq req)
        {
            return new S2CQuestCancelNavigationQuestRes()
            {
                QuestScheduleId = req.QuestScheduleId
            };
        }
    }
}

