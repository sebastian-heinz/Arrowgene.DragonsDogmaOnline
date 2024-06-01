using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetItemInfoHandler : GameRequestPacketHandler<C2SBazaarGetItemInfoReq, S2CBazaarGetItemInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetItemInfoHandler));
        
        public BazaarGetItemInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarGetItemInfoRes Handle(GameClient client, C2SBazaarGetItemInfoReq request)
        {            
            S2CBazaarGetItemInfoRes res = new S2CBazaarGetItemInfoRes()
            {
                BazaarItemList = Server.BazaarManager.GetActiveExhibitionsForItemId(request.ItemId, client.Character)
                    .Select(exhibition => exhibition.Info.ItemInfo)
                    .ToList()
            };
            return res;
        }
    }
}