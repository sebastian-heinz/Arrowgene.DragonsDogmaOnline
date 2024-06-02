using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftStartEquipGradeUpUnk0Unk0
    {

        public CDataCraftStartEquipGradeUpUnk0Unk0()
        {
            Unk0 = new List<Unk0DataList>();
        }
        public List<Unk0DataList> Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public bool Unk4 { get; set; }


        public class Unk0DataList
        {
            public byte ByteValue { get; set; }
            public uint UIntValue { get; set; }
            public ushort UShortValue1 { get; set; }
            public ushort UShortValue2 { get; set; }
            public bool BoolValue { get; set; }
        }

        public class Serializer : EntitySerializer<CDataCraftStartEquipGradeUpUnk0Unk0>
        {
            public override void Write(IBuffer buffer, CDataCraftStartEquipGradeUpUnk0Unk0 obj)
            {
                WriteEntityList<Unk0DataList>(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt16(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
            }

            public override CDataCraftStartEquipGradeUpUnk0Unk0 Read(IBuffer buffer)
            {
                    CDataCraftStartEquipGradeUpUnk0Unk0 obj = new CDataCraftStartEquipGradeUpUnk0Unk0();
                    obj.Unk0 = ReadEntityList<Unk0DataList>(buffer);
                    obj.Unk1 = ReadByte(buffer);
                    obj.Unk2 = ReadByte(buffer);
                    obj.Unk3 = ReadByte(buffer);
                    obj.Unk4 = ReadBool(buffer);
                    return obj;
            }
        }
    }
}
