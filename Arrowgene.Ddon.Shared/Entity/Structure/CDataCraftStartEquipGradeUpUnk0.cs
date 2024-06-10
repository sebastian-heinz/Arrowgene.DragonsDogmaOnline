using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftStartEquipGradeUpUnk0
    {

        public CDataCraftStartEquipGradeUpUnk0()
        {
            Unk0 = new List<CDataCraftStartEquipGradeUpUnk0Unk0>();
        }

        public List<CDataCraftStartEquipGradeUpUnk0Unk0> Unk0 { get; set; } // Potentially just dragon force related data?
        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public bool Unk4 { get; set; } // Displays a Dragon Froce icon on the enhance screen.

        public class Serializer : EntitySerializer<CDataCraftStartEquipGradeUpUnk0>
        {
            public override void Write(IBuffer buffer, CDataCraftStartEquipGradeUpUnk0 obj)
            {
                WriteEntityList<CDataCraftStartEquipGradeUpUnk0Unk0>(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteByte(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
            }

            public override CDataCraftStartEquipGradeUpUnk0 Read(IBuffer buffer)
            {
                    CDataCraftStartEquipGradeUpUnk0 obj = new CDataCraftStartEquipGradeUpUnk0();
                    obj.Unk0 = ReadEntityList<CDataCraftStartEquipGradeUpUnk0Unk0>(buffer);
                    obj.Unk1 = ReadByte(buffer);
                    obj.Unk2 = ReadByte(buffer);
                    obj.Unk3 = ReadByte(buffer);
                    obj.Unk4 = ReadBool(buffer);
                    return obj;
                
            }
        }
    }
}
