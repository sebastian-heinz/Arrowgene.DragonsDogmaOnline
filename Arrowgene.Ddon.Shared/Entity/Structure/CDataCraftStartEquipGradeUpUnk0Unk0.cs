using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftStartEquipGradeUpUnk0Unk0
    {
        // This might be entirely DragonAugment related
        // TODO: Implement DragonAugment to further test & validate.
        public CDataCraftStartEquipGradeUpUnk0Unk0()
        {
            Unk0 = 0;
            Unk1 = 0;
            Unk2 = 0;
            Unk3 = 0;
            IsMax = false;
        }

        public byte Unk0 { get; set; } // Maybe the DragonAugment Type?
        public uint Unk1 { get; set; }  // Maybe the DragonAugment ID? Can't tell.
        public ushort Unk2 { get; set; } // Filling this above 0 prevents the "UP" display? Unknown
        public ushort Unk3 { get; set; } // Displays "UP" next to the DragonAugment upon succesful enhance, probably some ID for dragonforce levels?
        public bool IsMax { get; set; } // Displays "MAX" next to the DragonAugment icon.
        public class Serializer : EntitySerializer<CDataCraftStartEquipGradeUpUnk0Unk0>
        {
            public override void Write(IBuffer buffer, CDataCraftStartEquipGradeUpUnk0Unk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt16(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
                WriteBool(buffer, obj.IsMax);
            }

            public override CDataCraftStartEquipGradeUpUnk0Unk0 Read(IBuffer buffer)
            {
                    CDataCraftStartEquipGradeUpUnk0Unk0 obj = new CDataCraftStartEquipGradeUpUnk0Unk0();
                    obj.Unk0 = ReadByte(buffer);
                    obj.Unk1 = ReadUInt32(buffer);
                    obj.Unk2 = ReadUInt16(buffer);
                    obj.Unk3 = ReadUInt16(buffer);
                    obj.IsMax = ReadBool(buffer);
                    return obj;
            }
        }
    }
}
