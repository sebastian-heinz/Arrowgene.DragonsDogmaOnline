using Arrowgene.Ddon.GameServer.Shop;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ShopGetShopGoodsListHandler : GameStructurePacketHandler<C2SShopGetShopGoodsListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ShopGetShopGoodsListHandler));
        
        private readonly ShopManager ShopManager;

        public ShopGetShopGoodsListHandler(DdonGameServer server) : base(server)
        {
            ShopManager = server.ShopManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SShopGetShopGoodsListReq> packet)
        {
            client.Character.LastEnteredShopId = packet.Structure.ShopId;

            S2CShopGetShopGoodsListRes res = ShopManager.GetAssets(packet.Structure.ShopId);
            client.Send(res);
        }
    }
}