using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLotCrestUnk2
    {
        public CDataDispelLotCrestUnk2()
        {
        }

        public ushort Unk0 { get; set; }
        public byte Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataDispelLotCrestUnk2>
        {
            public override void Write(IBuffer buffer, CDataDispelLotCrestUnk2 obj)
            {
                WriteUInt16(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override CDataDispelLotCrestUnk2 Read(IBuffer buffer)
            {
                CDataDispelLotCrestUnk2 obj = new CDataDispelLotCrestUnk2();
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}

