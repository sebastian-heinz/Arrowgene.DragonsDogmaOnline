using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetFavoriteWarpPointListHandler : GameRequestPacketHandler<C2SWarpGetFavoriteWarpPointListReq, S2CWarpGetFavoriteWarpPointListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetFavoriteWarpPointListHandler));

        public WarpGetFavoriteWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpGetFavoriteWarpPointListRes Handle(GameClient client, C2SWarpGetFavoriteWarpPointListReq request)
        {
            // Requested when using the Rift Teleport menu option
            return new S2CWarpGetFavoriteWarpPointListRes()
            {
                SlotIdMax = client.Character.FavWarpSlotNum,
                FavoriteWarpPointList = client.Character.ReleasedWarpPoints.Select(rwp => new CDataFavoriteWarpPoint()
                {
                    WarpPointId = rwp.WarpPointId,
                    Price = Server.AssetRepository.WarpPoints.Where(wp => wp.WarpPointId == rwp.WarpPointId).Single().CalculateFinalPrice(rwp.IsFavorite),
                    SlotNo = rwp.FavoriteSlotNo
                }).ToList()
            };
        }
    }
}
