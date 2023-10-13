using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetFavoriteWarpPointListHandler : StructurePacketHandler<GameClient, C2SWarpGetFavoriteWarpPointListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetFavoriteWarpPointListHandler));


        public WarpGetFavoriteWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpGetFavoriteWarpPointListReq> packet)
        {
            // Requested when using the Rift Teleport menu option
            client.Send(new S2CWarpGetFavoriteWarpPointListRes()
            {
                SlotIdMax = client.Character.FavWarpSlotNum,
                FavoriteWarpPointList = client.Character.ReleasedWarpPoints.Select(rwp => new CDataFavoriteWarpPoint()
                {
                    WarpPointId = rwp.WarpPointId,
                    Price = Server.AssetRepository.WarpPoints.Where(wp => wp.WarpPointId == rwp.WarpPointId).Single().CalculateFinalPrice(rwp.IsFavorite),
                    SlotNo = rwp.FavoriteSlotNo
                }).ToList()
            });
        }
    }
}
