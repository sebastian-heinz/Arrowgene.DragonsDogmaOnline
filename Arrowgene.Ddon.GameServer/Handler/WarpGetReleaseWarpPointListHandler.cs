using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetReleaseWarpPointListHandler : StructurePacketHandler<GameClient, C2SWarpGetReleaseWarpPointListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetReleaseWarpPointListHandler));


        public WarpGetReleaseWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpGetReleaseWarpPointListReq> packet)
        {
            S2CWarpGetReleaseWarpPointListRes res = new S2CWarpGetReleaseWarpPointListRes();
            client.Send(res);
        }
    }
}
