using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnSetFavoritePawnHandler : GameRequestPacketHandler<C2SPawnSetFavoritePawnReq, S2CPawnSetFavoritePawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSetFavoritePawnHandler));

        public PawnSetFavoritePawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnSetFavoritePawnRes Handle(GameClient client, C2SPawnSetFavoritePawnReq request)
        {
            if (!client.Character.FavoritedPawnIds.Contains(request.PawnId))
            {
                Server.Database.InsertPawnFavorite(client.Character.CharacterId, request.PawnId);
                client.Character.FavoritedPawnIds.Add(request.PawnId);
            }
            return new();
        }
    }
}
