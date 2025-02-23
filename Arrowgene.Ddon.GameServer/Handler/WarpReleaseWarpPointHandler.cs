using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpReleaseWarpPointHandler : GameRequestPacketHandler<C2SWarpReleaseWarpPointReq, S2CWarpReleaseWarpPointRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpReleaseWarpPointHandler));

        public WarpReleaseWarpPointHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpReleaseWarpPointRes Handle(GameClient client, C2SWarpReleaseWarpPointReq request)
        {
            ReleasedWarpPoint rwp = new ReleasedWarpPoint()
            {
                WarpPointId = request.WarpPointId,
                // WDT must ALWAYS be the first favorite, otherwise the client doesn't behave properly
                FavoriteSlotNo = request.WarpPointId == 1 ? 1u : 0u
            };

            // TODO: Check against MSQ progress to block S2/S3 warps.

            bool inserted = Server.Database.InsertIfNotExistsReleasedWarpPoint(client.Character.CharacterId, rwp);
            if (inserted)
            {
                client.Character.ReleasedWarpPoints.Add(rwp);
            }

            return new S2CWarpReleaseWarpPointRes
            {
                WarpPointId = request.WarpPointId
            };
        }
    }
}
