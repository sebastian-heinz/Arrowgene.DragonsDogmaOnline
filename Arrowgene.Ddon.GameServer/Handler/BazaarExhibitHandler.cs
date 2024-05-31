using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarExhibitHandler : GameRequestPacketHandler<C2SBazaarExhibitReq, S2CBazaarExhibitRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarExhibitHandler));
        
        public BazaarExhibitHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarExhibitRes Handle(GameClient client, C2SBazaarExhibitReq request)
        {
            ulong bazaarId = Server.BazaarManager.Exhibit(client, request.StorageType, request.ItemUID, (ushort) request.Num, request.Price, request.Flag);
            return new S2CBazaarExhibitRes()
            {
                BazaarId = bazaarId
            };
        }
    }
}