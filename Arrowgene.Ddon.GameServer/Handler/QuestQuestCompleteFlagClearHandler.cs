using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestCompleteFlagClearHandler : GameRequestPacketHandler<C2SQuestQuestCompleteFlagClearReq, S2CQuestQuestCompleteFlagClearRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestCompleteFlagClearHandler));

        public QuestQuestCompleteFlagClearHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestQuestCompleteFlagClearRes Handle(GameClient client, C2SQuestQuestCompleteFlagClearReq request)
        {
            return new S2CQuestQuestCompleteFlagClearRes()
            {
                QuestScheduleId = request.QuestScheduleId
            };
        }
    }
}
