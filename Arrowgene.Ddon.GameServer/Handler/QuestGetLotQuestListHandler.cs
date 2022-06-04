using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetLotQuestListHandler : StructurePacketHandler<GameClient, C2SQuestGetLotQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetLotQuestListHandler));

        public QuestGetLotQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetLotQuestListReq> req)
        {
            S2CQuestGetLotQuestListRes res = new S2CQuestGetLotQuestListRes();
            client.Send(res);
        }
    }
}
