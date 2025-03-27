using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipEnhancedEnhanceItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_ENHANCED_ENHANCE_ITEM_RES;

        public S2CEquipEnhancedEnhanceItemRes()
        {
            Unk0 = string.Empty;
        }

        public string Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public ushort Unk4 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipEnhancedEnhanceItemRes>
        {
            public override void Write(IBuffer buffer, S2CEquipEnhancedEnhanceItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteMtString(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteByte(buffer, obj.Unk3);
                WriteUInt16(buffer, obj.Unk4);
            }

            public override S2CEquipEnhancedEnhanceItemRes Read(IBuffer buffer)
            {
                S2CEquipEnhancedEnhanceItemRes obj = new S2CEquipEnhancedEnhanceItemRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadMtString(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.Unk4 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
