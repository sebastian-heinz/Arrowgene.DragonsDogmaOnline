using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftStartEquipGradeUpUnk0Unk0
    {
        // This might be entirely Dragon Force related
        // TODO: Implement Dragon Force to further test & validate.
        public CDataCraftStartEquipGradeUpUnk0Unk0()
        {
        }

        public byte Unk0 { get; set; } // Maybe the Dragon Force Type?
        public uint Unk1 { get; set; }  // Maybe the Dragon Force ID? Can't tell.
        public ushort Unk2 { get; set; } // Filling this above 0 prevents the "UP" display? Unknown
        public ushort Unk3 { get; set; } // Displays "UP" next to the Dragon Force upon succesful enhance, probably some ID for dragonforce levels?
        public bool IsMax { get; set; } // Displays "MAX" next to the Dragon Force icon.


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
