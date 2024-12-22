using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaRankUnk0
    {
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public ulong Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataAreaRankUnk0>
        {
            public override void Write(IBuffer buffer, CDataAreaRankUnk0 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt64(buffer, obj.Unk2);
            }

            public override CDataAreaRankUnk0 Read(IBuffer buffer)
            {
                CDataAreaRankUnk0 obj = new CDataAreaRankUnk0();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
