using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDatapresetEquipUnk0
    {
        public CDatapresetEquipUnk0()
        {
        }

        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public ushort Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDatapresetEquipUnk0>
        {
            public override void Write(IBuffer buffer, CDatapresetEquipUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt16(buffer, obj.Unk2);
            }

            public override CDatapresetEquipUnk0 Read(IBuffer buffer)
            {
                CDatapresetEquipUnk0 obj = new CDatapresetEquipUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
