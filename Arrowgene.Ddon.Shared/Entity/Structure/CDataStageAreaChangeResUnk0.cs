using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageAreaChangeResUnk0
    {
        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public byte Unk4 { get; set; }

        public class Serializer : EntitySerializer<CDataStageAreaChangeResUnk0>
        {
            public override void Write(IBuffer buffer, CDataStageAreaChangeResUnk0 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteByte(buffer, obj.Unk4);
            }

            public override CDataStageAreaChangeResUnk0 Read(IBuffer buffer)
            {
                CDataStageAreaChangeResUnk0 obj = new CDataStageAreaChangeResUnk0();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
