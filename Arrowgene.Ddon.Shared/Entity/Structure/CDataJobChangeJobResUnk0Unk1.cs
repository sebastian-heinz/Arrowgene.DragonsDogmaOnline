using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobChangeJobResUnk0Unk1
    {
        public CDataJobChangeJobResUnk0Unk1()
        {
            Unk0=0;
            Unk1=0;
        }

        public byte Unk0;
        public ushort Unk1;

        public class Serializer : EntitySerializer<CDataJobChangeJobResUnk0Unk1>
        {
            public override void Write(IBuffer buffer, CDataJobChangeJobResUnk0Unk1 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
            }

            public override CDataJobChangeJobResUnk0Unk1 Read(IBuffer buffer)
            {
                CDataJobChangeJobResUnk0Unk1 obj = new CDataJobChangeJobResUnk0Unk1();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
