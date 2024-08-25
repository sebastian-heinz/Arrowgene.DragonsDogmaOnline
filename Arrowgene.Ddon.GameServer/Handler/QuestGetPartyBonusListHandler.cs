using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPartyBonusListHandler : GameRequestPacketHandler<C2SQuestGetPartyBonusListReq, S2CQuestGetPartyBonusListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPartyBonusListHandler));

        public QuestGetPartyBonusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetPartyBonusListRes Handle(GameClient client, C2SQuestGetPartyBonusListReq request)
        {
            return new S2CQuestGetPartyBonusListRes();
        }
    }
}
