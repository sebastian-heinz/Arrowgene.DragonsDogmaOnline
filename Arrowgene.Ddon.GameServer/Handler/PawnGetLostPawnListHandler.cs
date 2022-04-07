using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetLostPawnListHandler : StructurePacketHandler<GameClient, C2SPawnGetLostPawnListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetLostPawnListHandler));

        public PawnGetLostPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetLostPawnListReq> req)
        {
            S2CPawnGetLostPawnListRes res = new S2CPawnGetLostPawnListRes();
            client.Send(res);
        }
    }
}
