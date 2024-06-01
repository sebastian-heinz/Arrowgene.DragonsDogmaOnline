using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarGetItemPriceLimitRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_GET_ITEM_PRICE_LIMIT_RES;

        public uint ItemId { get; set; }
        public uint Low { get; set; }
        public uint High { get; set; }
        public ushort Num { get; set; }


        public class Serializer : PacketEntitySerializer<S2CBazaarGetItemPriceLimitRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarGetItemPriceLimitRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.Low);
                WriteUInt32(buffer, obj.High);
                WriteUInt16(buffer, obj.Num);
            }

            public override S2CBazaarGetItemPriceLimitRes Read(IBuffer buffer)
            {
                S2CBazaarGetItemPriceLimitRes obj = new S2CBazaarGetItemPriceLimitRes();
                ReadServerResponse(buffer, obj);
                obj.ItemId = ReadUInt32(buffer);
                obj.Low = ReadUInt32(buffer);
                obj.High = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}