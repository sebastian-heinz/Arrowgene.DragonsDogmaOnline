using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetPawnHistoryListHandler : StructurePacketHandler<GameClient, C2SPawnGetPawnHistoryListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPawnHistoryListHandler));

        public PawnGetPawnHistoryListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetPawnHistoryListReq> req)
        {
            S2CPawnGetPawnHistoryListRes res = new S2CPawnGetPawnHistoryListRes();
            res.PawnId = req.Structure;
            client.Send(res);
        }
    }
}
