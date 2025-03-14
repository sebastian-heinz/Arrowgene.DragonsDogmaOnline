#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpRegisterFavoriteWarpHandler : GameRequestPacketHandler<C2SWarpRegisterFavoriteWarpReq, S2CWarpRegisterFavoriteWarpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpRegisterFavoriteWarpHandler));

        public WarpRegisterFavoriteWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpRegisterFavoriteWarpRes Handle(GameClient client, C2SWarpRegisterFavoriteWarpReq request)
        {
            // TODO: Run in transaction
            ReleasedWarpPoint? oldFavorite = client.Character.ReleasedWarpPoints.Where(rwp => rwp.FavoriteSlotNo == request.SlotNo).SingleOrDefault();
            if (oldFavorite != null)
            {
                oldFavorite.FavoriteSlotNo = 0;
                Server.Database.UpdateReleasedWarpPoint(client.Character.CharacterId, oldFavorite);
            }
            else
            {
                oldFavorite = new ReleasedWarpPoint()
                {
                    WarpPointId = request.WarpPointId,
                    FavoriteSlotNo = request.SlotNo
                };
            }

            ReleasedWarpPoint newFavorite = client.Character.ReleasedWarpPoints.Where(rwp => rwp.WarpPointId == request.WarpPointId).Single();
            newFavorite.FavoriteSlotNo = request.SlotNo;
            Server.Database.UpdateReleasedWarpPoint(client.Character.CharacterId, newFavorite);
            
            return new S2CWarpRegisterFavoriteWarpRes
            {
                WarpPointId = request.WarpPointId,
                SlotNo = request.SlotNo
            };
        }
    }
}
