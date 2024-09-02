using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarReExhibitHandler : GameRequestPacketHandler<C2SBazaarReExhibitReq, S2CBazaarReExhibitRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarReExhibitHandler));
        
        public BazaarReExhibitHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarReExhibitRes Handle(GameClient client, C2SBazaarReExhibitReq request)
        {
            ulong newBazaarId = Server.BazaarManager.ReExhibit(request.BazaarId, request.Price);
            return new S2CBazaarReExhibitRes()
            {
                BazaarId = newBazaarId
            };
        }
    }
}