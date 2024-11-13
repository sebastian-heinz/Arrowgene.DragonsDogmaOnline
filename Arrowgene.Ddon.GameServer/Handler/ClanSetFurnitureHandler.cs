using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanSetFurnitureHandler : GameRequestPacketHandler<C2SClanSetFurnitureReq, S2CClanSetFurnitureRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanSetFurnitureHandler));

        public ClanSetFurnitureHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanSetFurnitureRes Handle(GameClient client, C2SClanSetFurnitureReq request)
        {
            // TODO: Track in the database.
            return new();
        }
    }
}
