using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CShopBuyShopGoodsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SHOP_BUY_SHOP_GOODS_RES;

        public WalletType PointType { get; set; } // I think?

        public class Serializer : PacketEntitySerializer<S2CShopBuyShopGoodsRes>
        {
            public override void Write(IBuffer buffer, S2CShopBuyShopGoodsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.PointType);
            }

            public override S2CShopBuyShopGoodsRes Read(IBuffer buffer)
            {
                S2CShopBuyShopGoodsRes obj = new S2CShopBuyShopGoodsRes();
                ReadServerResponse(buffer, obj);
                obj.PointType = (WalletType) ReadByte(buffer);
                return obj;
            }
        }
    }
}