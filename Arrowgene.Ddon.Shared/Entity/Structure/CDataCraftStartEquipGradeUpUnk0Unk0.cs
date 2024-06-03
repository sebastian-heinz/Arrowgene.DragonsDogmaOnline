using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftStartEquipGradeUpUnk0Unk0
    {

        public CDataCraftStartEquipGradeUpUnk0Unk0()
        {
        }

        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public ushort Unk2 { get; set; }
        public ushort Unk3 { get; set; }
        public bool Unk4 { get; set; }


        public class Serializer : EntitySerializer<CDataCraftStartEquipGradeUpUnk0Unk0>
        {
            public override void Write(IBuffer buffer, CDataCraftStartEquipGradeUpUnk0Unk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt16(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
            }

            public override CDataCraftStartEquipGradeUpUnk0Unk0 Read(IBuffer buffer)
            {
                    CDataCraftStartEquipGradeUpUnk0Unk0 obj = new CDataCraftStartEquipGradeUpUnk0Unk0();
                    obj.Unk0 = ReadByte(buffer);
                    obj.Unk1 = ReadUInt32(buffer);
                    obj.Unk2 = ReadUInt16(buffer);
                    obj.Unk3 = ReadUInt16(buffer);
                    obj.Unk4 = ReadBool(buffer);
                    return obj;
            }
        }
    }
}
