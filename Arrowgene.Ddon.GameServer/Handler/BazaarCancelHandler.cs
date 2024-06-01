using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarCancelHandler : GameRequestPacketHandler<C2SBazaarCancelReq, S2CBazaarCancelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarCancelHandler));
        
        public BazaarCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarCancelRes Handle(GameClient client, C2SBazaarCancelReq request)
        {
            Server.BazaarManager.Cancel(client, request.BazaarId);
            return new S2CBazaarCancelRes()
            {
                BazaarId = request.BazaarId
            };
        }
    }
}