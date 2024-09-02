using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageAreaChangeResUnk1
    {

        public uint Unk0 { get; set; }
        public bool Unk1 { get; set; }
        public bool Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataStageAreaChangeResUnk1>
        {
            public override void Write(IBuffer buffer, CDataStageAreaChangeResUnk1 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
                WriteBool(buffer, obj.Unk2);
            }

            public override CDataStageAreaChangeResUnk1 Read(IBuffer buffer)
            {
                CDataStageAreaChangeResUnk1 obj = new CDataStageAreaChangeResUnk1();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.Unk2 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
