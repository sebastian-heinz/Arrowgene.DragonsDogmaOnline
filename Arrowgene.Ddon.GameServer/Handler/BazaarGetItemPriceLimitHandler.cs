using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetItemPriceLimitHandler : GameRequestPacketHandler<C2SBazaarGetItemPriceLimitReq, S2CBazaarGetItemPriceLimitRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetItemPriceLimitHandler));
        
        public BazaarGetItemPriceLimitHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarGetItemPriceLimitRes Handle(GameClient client, C2SBazaarGetItemPriceLimitReq request)
        {
            // TODO: Take values from itemlist.csv?
            return new S2CBazaarGetItemPriceLimitRes()
            {
                ItemId = request.ItemId,
                Low = uint.MinValue,
                High = int.MaxValue, // Has to be int, apparently the client understands this field as an int, a number too high is seen as negative
                Num = ushort.MaxValue // I recall it being 10 in most of the cases
            };
        }
    }
}