#nullable enable
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpRegisterFavoriteWarpHandler : StructurePacketHandler<GameClient, C2SWarpRegisterFavoriteWarpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpRegisterFavoriteWarpHandler));

        public WarpRegisterFavoriteWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpRegisterFavoriteWarpReq> request)
        {
            // TODO: Run in transaction
            ReleasedWarpPoint? oldFavorite = client.Character.ReleasedWarpPoints.Where(rwp => rwp.FavoriteSlotNo == request.Structure.SlotNo).SingleOrDefault();
            if (oldFavorite != null)
            {
                oldFavorite.FavoriteSlotNo = 0;
                Server.Database.UpdateReleasedWarpPoint(client.Character.CharacterId, oldFavorite);
            }

            ReleasedWarpPoint newFavorite = client.Character.ReleasedWarpPoints.Where(rwp => rwp.WarpPointId == request.Structure.WarpPointId).Single();
            newFavorite.FavoriteSlotNo = request.Structure.SlotNo;
            Server.Database.UpdateReleasedWarpPoint(client.Character.CharacterId, newFavorite);
            
            client.Send(new S2CWarpRegisterFavoriteWarpRes
            {
                WarpPointId = request.Structure.WarpPointId,
                SlotNo = request.Structure.SlotNo
            });
        }
    }
}