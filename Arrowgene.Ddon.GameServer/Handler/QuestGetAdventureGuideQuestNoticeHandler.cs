using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetAdventureGuideQuestNoticeHandler : GameRequestPacketHandler<C2SQuestGetAdventureGuideQuestNtcReq, S2CQuestGetAdventureGuideQuestNtcRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetAdventureGuideQuestNoticeHandler));

        public QuestGetAdventureGuideQuestNoticeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetAdventureGuideQuestNtcRes Handle(GameClient client, C2SQuestGetAdventureGuideQuestNtcReq request)
        {
            // Bool Has new quest?
            return new S2CQuestGetAdventureGuideQuestNtcRes();
        }
    }
}
