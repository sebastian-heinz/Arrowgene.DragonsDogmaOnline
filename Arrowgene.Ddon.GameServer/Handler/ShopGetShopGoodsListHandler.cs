using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ShopGetShopGoodsListHandler : GameStructurePacketHandler<C2SShopGetShopGoodsListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ShopGetShopGoodsListHandler));
        
        public ShopGetShopGoodsListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SShopGetShopGoodsListReq> packet)
        {
            client.Character.LastEnteredShopId = packet.Structure.ShopId;

            S2CShopGetShopGoodsListRes res = client.InstanceShopManager.GetAssets(packet.Structure.ShopId);
            client.Send(res);
        }
    }
}