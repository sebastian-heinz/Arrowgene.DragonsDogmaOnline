using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Shop
{
    public class ShopManager : AssetManager<Shared.Model.Shop>
    {
        protected Dictionary<uint, S2CShopGetShopGoodsListRes> Goods;

        public ShopManager(AssetRepository assetRepository, IDatabase database) : base(assetRepository, AssetRepository.ShopKey, database, assetRepository.ShopAsset)
        {
        }

        protected override void OnInit()
        {
            Goods = new Dictionary<uint, S2CShopGetShopGoodsListRes>();
        }

        public override void Load()
        {
            Goods.Clear();
            foreach (Shared.Model.Shop shop in this._assetList)
            {
                Goods.Add(shop.ShopId, shop.Data);
            }
        }

        public S2CShopGetShopGoodsListRes GetAssets(uint ShopId)
        {
            return Goods.GetValueOrDefault(ShopId, new S2CShopGetShopGoodsListRes());
        }
    }
}