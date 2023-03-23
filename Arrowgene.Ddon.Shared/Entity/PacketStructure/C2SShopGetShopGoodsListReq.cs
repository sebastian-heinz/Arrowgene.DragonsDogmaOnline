using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SShopGetShopGoodsListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SHOP_GET_SHOP_GOODS_LIST_REQ;

        public uint ShopId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SShopGetShopGoodsListReq>
        {
            public override void Write(IBuffer buffer, C2SShopGetShopGoodsListReq obj)
            {
                WriteUInt32(buffer, obj.ShopId);
            }

            public override C2SShopGetShopGoodsListReq Read(IBuffer buffer)
            {
                C2SShopGetShopGoodsListReq obj = new C2SShopGetShopGoodsListReq();
                obj.ShopId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}