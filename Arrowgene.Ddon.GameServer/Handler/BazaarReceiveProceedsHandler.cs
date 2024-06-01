using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarReceiveProceedsHandler : GameRequestPacketHandler<C2SBazaarReceiveProceedsReq, S2CBazaarReceiveProceedsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarReceiveProceedsHandler));
        
        public BazaarReceiveProceedsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarReceiveProceedsRes Handle(GameClient client, C2SBazaarReceiveProceedsReq request)
        {
            uint totalProceeds = Server.BazaarManager.ReceiveProceeds(client);
            return new S2CBazaarReceiveProceedsRes()
            {
                Proceeds = totalProceeds
            };
            
        }
    }
}