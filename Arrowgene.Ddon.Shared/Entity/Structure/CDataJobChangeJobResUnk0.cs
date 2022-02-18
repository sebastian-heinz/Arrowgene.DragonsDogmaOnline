using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobChangeJobResUnk0
    {
        public CDataJobChangeJobResUnk0()
        {
            Unk0=0;
            Unk1=new List<CDataJobChangeJobResUnk0Unk1>();
        }

        public byte Unk0 { get; set; }
        public List<CDataJobChangeJobResUnk0Unk1> Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataJobChangeJobResUnk0>
        {
            public override void Write(IBuffer buffer, CDataJobChangeJobResUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteEntityList<CDataJobChangeJobResUnk0Unk1>(buffer, obj.Unk1);
            }

            public override CDataJobChangeJobResUnk0 Read(IBuffer buffer)
            {
                CDataJobChangeJobResUnk0 obj = new CDataJobChangeJobResUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadEntityList<CDataJobChangeJobResUnk0Unk1>(buffer);
                return obj;
            }
        }
    }
}