using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftCancelCraftHandler : GameRequestPacketHandler<C2SCraftCancelCraftReq, C2SCraftCancelCraftRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftCancelCraftHandler));

        public CraftCancelCraftHandler(DdonGameServer server) : base(server)
        {
        }

        public override C2SCraftCancelCraftRes Handle(GameClient client, C2SCraftCancelCraftReq request)
        {
            Server.Database.DeletePawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID);

            return new C2SCraftCancelCraftRes();
        }
    }
}
