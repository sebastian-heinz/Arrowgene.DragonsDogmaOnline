using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ShopGetShopGoodsListHandler : GameRequestPacketHandler<C2SShopGetShopGoodsListReq, S2CShopGetShopGoodsListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ShopGetShopGoodsListHandler));
        
        public ShopGetShopGoodsListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CShopGetShopGoodsListRes Handle(GameClient client, C2SShopGetShopGoodsListReq request)
        {
            client.Character.LastEnteredShopId = request.ShopId;

            return client.InstanceShopManager.GetAssets(request.ShopId);
        }

    }
}
