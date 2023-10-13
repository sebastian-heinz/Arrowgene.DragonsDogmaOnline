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
    public class WarpGetWarpPointListHandler : StructurePacketHandler<GameClient, C2SWarpGetWarpPointListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));


        public WarpGetWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpGetWarpPointListReq> packet)
        {
            // Requested when interacting with a portcrystal
            client.Send(new S2CWarpGetWarpPointListRes()
            {
                WarpPointList = client.Character.ReleasedWarpPoints.Select(rwp => new CDataWarpPoint()
                {
                    Id = rwp.WarpPointId,
                    RimPrice = Server.AssetRepository.WarpPoints.Where(wp => wp.WarpPointId == rwp.WarpPointId).Single().CalculateFinalPrice(rwp.IsFavorite)
                }).ToList()
            });
        }
    }
}
