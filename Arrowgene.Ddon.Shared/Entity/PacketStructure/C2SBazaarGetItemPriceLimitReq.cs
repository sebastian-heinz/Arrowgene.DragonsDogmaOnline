using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarGetItemPriceLimitReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_GET_ITEM_PRICE_LIMIT_REQ;

        public uint ItemId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarGetItemPriceLimitReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarGetItemPriceLimitReq obj)
            {
                WriteUInt32(buffer, obj.ItemId);                
            }

            public override C2SBazaarGetItemPriceLimitReq Read(IBuffer buffer)
            {
                C2SBazaarGetItemPriceLimitReq obj = new C2SBazaarGetItemPriceLimitReq();
                obj.ItemId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}