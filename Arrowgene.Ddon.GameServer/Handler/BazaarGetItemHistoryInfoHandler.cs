using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetItemHistoryInfoHandler : GameRequestPacketHandler<C2SBazaarGetItemHistoryInfoReq, S2CBazaarGetItemHistoryInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetItemHistoryInfoHandler));
        
        public BazaarGetItemHistoryInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarGetItemHistoryInfoRes Handle(GameClient client, C2SBazaarGetItemHistoryInfoReq request)
        {
            return new S2CBazaarGetItemHistoryInfoRes();
        }
    }
}