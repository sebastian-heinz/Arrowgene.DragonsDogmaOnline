using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetReleaseWarpPointListHandler : GameRequestPacketHandler<C2SWarpGetReleaseWarpPointListReq, S2CWarpGetReleaseWarpPointListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetReleaseWarpPointListHandler));

        public WarpGetReleaseWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpGetReleaseWarpPointListRes Handle(GameClient client, C2SWarpGetReleaseWarpPointListReq request)
        {
            // I believe this is a list of already discovered teleporters?
            // When a player interacts with a TP that isn't in this list
            // a C2S_WARP_RELEASE_WARP_POINT_REQ request is sent.
            return new S2CWarpGetReleaseWarpPointListRes() {
                WarpPointIdList = client.Character.ReleasedWarpPoints.Select(wp => new CDataCommonU32(wp.WarpPointId)).ToList()
            };
        }

    }
}
