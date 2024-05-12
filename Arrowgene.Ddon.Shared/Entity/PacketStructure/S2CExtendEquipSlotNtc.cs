using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CExtendEquipSlotNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ITEM_EXTEND_EQUIP_SLOT_NTC;

        public EquipCategory EquipSlot { get; set; } // Is it really a category?
        public byte AddNum { get; set; }
        public byte TotalNum { get; set; }

        public class Serializer : PacketEntitySerializer<S2CExtendEquipSlotNtc>
        {
            public override void Write(IBuffer buffer, S2CExtendEquipSlotNtc obj)
            {
                WriteByte(buffer, (byte) obj.EquipSlot);
                WriteByte(buffer, obj.AddNum);
                WriteByte(buffer, obj.TotalNum);
            }

            public override S2CExtendEquipSlotNtc Read(IBuffer buffer)
            {
                S2CExtendEquipSlotNtc obj = new S2CExtendEquipSlotNtc();
                obj.EquipSlot = (EquipCategory) ReadByte(buffer);
                obj.AddNum = ReadByte(buffer);
                obj.TotalNum = ReadByte(buffer);
                return obj;
            }
        }
    }
}
