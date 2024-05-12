using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Shop
{
    public class InstanceShopManager
    {
        private readonly ShopManager _shopManager;
        private readonly Dictionary<uint, S2CShopGetShopGoodsListRes> _goods;

        public InstanceShopManager(ShopManager shopManager)
        {
            _shopManager = shopManager;
            _goods = new Dictionary<uint, S2CShopGetShopGoodsListRes>();
        }

        public S2CShopGetShopGoodsListRes GetAssets(uint shopId)
        {
            if(!_goods.ContainsKey(shopId))
            {
                S2CShopGetShopGoodsListRes shop = _shopManager.GetAssets(shopId);
                S2CShopGetShopGoodsListRes shopClone = (S2CShopGetShopGoodsListRes) shop.Clone();
                _goods.Add(shopId, shopClone);
                return shopClone;
            }

            return _goods[shopId];
        }

        public void Clear()
        {
            _goods.Clear();
        }
    }
}