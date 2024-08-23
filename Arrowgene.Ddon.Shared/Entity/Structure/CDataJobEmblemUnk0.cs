using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemUnk0
    {
        public CDataJobEmblemUnk0()
        {
            Unk0 = string.Empty;
        }

        public string Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemUnk0>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemUnk0 obj)
            {
                WriteMtString(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteByte(buffer, obj.Unk3);
            }

            public override CDataJobEmblemUnk0 Read(IBuffer buffer)
            {
                CDataJobEmblemUnk0 obj = new CDataJobEmblemUnk0();
                obj.Unk0 = ReadMtString(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
