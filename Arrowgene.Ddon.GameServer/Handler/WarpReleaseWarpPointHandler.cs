using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpReleaseWarpPointHandler : StructurePacketHandler<GameClient, C2SWarpReleaseWarpPointReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpReleaseWarpPointHandler));

        public WarpReleaseWarpPointHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpReleaseWarpPointReq> packet)
        {
            ReleasedWarpPoint rwp = new ReleasedWarpPoint()
            {
                WarpPointId = packet.Structure.WarpPointId,
                // WDT must ALWAYS be the first favorite, otherwise the client doesn't behave properly
                FavoriteSlotNo = packet.Structure.WarpPointId == 1 ? 1u : 0u
            };
            bool inserted = Server.Database.InsertIfNotExistsReleasedWarpPoint(client.Character.CharacterId, rwp);
            if(inserted)
            {
                client.Character.ReleasedWarpPoints.Add(rwp);
            }

            client.Send(new S2CWarpReleaseWarpPointRes
            {
                WarpPointId = packet.Structure.WarpPointId
            });
        }
    }
}