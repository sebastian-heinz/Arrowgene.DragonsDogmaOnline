using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemUseBagItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_USE_BAG_ITEM_REQ;

        public string ItemUId { get; set; }
        public uint Amount { get; set; }
        
        public C2SItemUseBagItemReq()
        {
            ItemUId = "";
            Amount = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SItemUseBagItemReq>
        {
            
            public override void Write(IBuffer buffer, C2SItemUseBagItemReq obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.Amount);
            }

            public override C2SItemUseBagItemReq Read(IBuffer buffer)
            {
                C2SItemUseBagItemReq obj = new C2SItemUseBagItemReq();
                obj.ItemUId = ReadMtString(buffer);
                obj.Amount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
