using System.Runtime.Serialization;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Shop
    {
        public Shop()
        {
            Data = new S2CShopGetShopGoodsListRes();
        }

        public uint ShopId { get; set; }
        public S2CShopGetShopGoodsListRes Data { get; set; }
    }
}