using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetStartPointListHandler : GameRequestPacketHandler<C2SWarpGetStartPointListReq, S2CWarpGetStartPointListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetStartPointListHandler));

        public WarpGetStartPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpGetStartPointListRes Handle(GameClient client, C2SWarpGetStartPointListReq request)
        {
            S2CWarpGetStartPointListRes res = new S2CWarpGetStartPointListRes();

            // Check for Megadosis; if you return a blank list it defaults to WDT.
            if (client.Character.ReleasedWarpPoints.Any(x => x.WarpPointId == 0x57))
            {
                res.WarpPointIdList.Add(new CDataCommonU32(0x01)); // White Dragon Temple
                res.WarpPointIdList.Add(new CDataCommonU32(0x57)); // Megadosis
            }

            return res;
        }
    }
}
