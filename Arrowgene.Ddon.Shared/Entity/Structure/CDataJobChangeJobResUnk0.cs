using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobChangeJobResUnk0
    {
        public CDataJobChangeJobResUnk0()
        {
            Unk0=0;
            Unk1=new List<CDataCharacterItemSlotInfo>();
        }

        public byte Unk0 { get; set; }
        public List<CDataCharacterItemSlotInfo> Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataJobChangeJobResUnk0>
        {
            public override void Write(IBuffer buffer, CDataJobChangeJobResUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteEntityList<CDataCharacterItemSlotInfo>(buffer, obj.Unk1);
            }

            public override CDataJobChangeJobResUnk0 Read(IBuffer buffer)
            {
                CDataJobChangeJobResUnk0 obj = new CDataJobChangeJobResUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadEntityList<CDataCharacterItemSlotInfo>(buffer);
                return obj;
            }
        }
    }
}
