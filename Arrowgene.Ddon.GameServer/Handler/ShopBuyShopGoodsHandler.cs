using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ShopBuyShopGoodsHandler : GameStructurePacketHandler<C2SShopBuyShopGoodsReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ShopBuyShopGoodsHandler));
        
        public ShopBuyShopGoodsHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SShopBuyShopGoodsReq> packet)
        {
            client.Send(new S2CShopBuyShopGoodsRes()
            {
                PointType = WalletType.Gold // ?
            });

            client.Send(new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = 0x10a
            });
        }
    }
}