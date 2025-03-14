using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetWarpPointListHandler : GameRequestPacketHandler<C2SWarpGetWarpPointListReq, S2CWarpGetWarpPointListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));

        public WarpGetWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpGetWarpPointListRes Handle(GameClient client, C2SWarpGetWarpPointListReq request)
        {
            // Requested when interacting with a portcrystal
            // TODO: Figure out warp pricing.
            return new S2CWarpGetWarpPointListRes()
            {
                WarpPointList = client.Character.ReleasedWarpPoints.Select(rwp => new CDataWarpPoint()
                {
                    Id = rwp.WarpPointId,
                    RimPrice = Server.AssetRepository.WarpPoints.Where(wp => wp.WarpPointId == rwp.WarpPointId).Single().CalculateFinalPrice(rwp.IsFavorite)
                }).ToList()
            };
        }
    }
}
