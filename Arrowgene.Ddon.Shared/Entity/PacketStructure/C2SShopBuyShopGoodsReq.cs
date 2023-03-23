using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SShopBuyShopGoodsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SHOP_BUY_SHOP_GOODS_REQ;

        public uint GoodsIndex { get; set; }
        public uint Num { get; set; }

        // Values found:
        // 19: Buying to bag
        // 20: Buying to storage
        // It could possibly be StorageType?
        public byte Destination { get; set; }
        public uint Price { get; set; }        

        public class Serializer : PacketEntitySerializer<C2SShopBuyShopGoodsReq>
        {
            public override void Write(IBuffer buffer, C2SShopBuyShopGoodsReq obj)
            {
                WriteUInt32(buffer, obj.GoodsIndex);
                WriteUInt32(buffer, obj.Num);
                WriteByte(buffer, obj.Destination);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SShopBuyShopGoodsReq Read(IBuffer buffer)
            {
                C2SShopBuyShopGoodsReq obj = new C2SShopBuyShopGoodsReq();
                obj.GoodsIndex = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.Destination = ReadByte(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}