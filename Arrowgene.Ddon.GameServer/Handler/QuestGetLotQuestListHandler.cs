using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetLotQuestListHandler : GameRequestPacketHandler<C2SQuestGetLotQuestListReq, S2CQuestGetLotQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetLotQuestListHandler));

        public QuestGetLotQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetLotQuestListRes Handle(GameClient client, C2SQuestGetLotQuestListReq request)
        {
            return new()
            {
                LotQuestType = request.LotQuestType
            };
        }
    }
}
