using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaRankUnk1
    {
        public uint Unk0 { get; set; }
        public bool Unk1 { get; set; }
        public ulong Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataAreaRankUnk1>
        {
            public override void Write(IBuffer buffer, CDataAreaRankUnk1 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
                WriteUInt64(buffer, obj.Unk2);
            }

            public override CDataAreaRankUnk1 Read(IBuffer buffer)
            {
                CDataAreaRankUnk1 obj = new CDataAreaRankUnk1();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.Unk2 = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
