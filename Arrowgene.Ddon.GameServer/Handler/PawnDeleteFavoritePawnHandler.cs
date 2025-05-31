using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnDeleteFavoritePawnHandler : GameRequestPacketHandler<C2SPawnDeleteFavoritePawnReq, S2CPawnDeleteFavoritePawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnDeleteFavoritePawnHandler));

        public PawnDeleteFavoritePawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnDeleteFavoritePawnRes Handle(GameClient client, C2SPawnDeleteFavoritePawnReq request)
        {
            Server.Database.DeletePawnFavorite(client.Character.CharacterId, request.PawnId);
            client.Character.FavoritedPawnIds.Remove(request.PawnId);
            return new();
        }
    }
}
