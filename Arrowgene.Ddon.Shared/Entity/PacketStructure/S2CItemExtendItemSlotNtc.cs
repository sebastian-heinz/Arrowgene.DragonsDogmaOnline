using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemExtendItemSlotNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ITEM_EXTEND_ITEM_SLOT_NTC;

        public S2CItemExtendItemSlotNtc()
        {
            Category=0;
            AddNum=0;
            TotalNum=0;
        }

        public byte Category { get; set; }
        public ushort AddNum { get; set; }
        public ushort TotalNum { get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemExtendItemSlotNtc>
        {
            public override void Write(IBuffer buffer, S2CItemExtendItemSlotNtc obj)
            {
                WriteByte(buffer, obj.Category);
                WriteUInt16(buffer, obj.AddNum);
                WriteUInt16(buffer, obj.TotalNum);
            }

            public override S2CItemExtendItemSlotNtc Read(IBuffer buffer)
            {
                S2CItemExtendItemSlotNtc obj = new S2CItemExtendItemSlotNtc();
                obj.Category = ReadByte(buffer);
                obj.AddNum = ReadUInt16(buffer);
                obj.TotalNum = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}