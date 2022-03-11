using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            // I believe this is a list of already discovered teleporters?
            // When a player interacts with a TP that isn't in this list
            // a C2S_WARP_RELEASE_WARP_POINT_REQ request is sent.
            S2CWarpGetReleaseWarpPointListRes res = new S2CWarpGetReleaseWarpPointListRes();
            res.WarpPointIdList.Add(new CDataCommonU32(0x01)); // White Dragon Temple
            client.Send(res);
        }
    }
}
