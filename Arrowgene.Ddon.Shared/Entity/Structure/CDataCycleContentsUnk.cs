using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsUnk
    {
        public uint Unk0 { get; set; }
        public long Unk1 { get; set; }
        public long Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataCycleContentsUnk>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsUnk obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteInt64(buffer, obj.Unk1);
                WriteInt64(buffer, obj.Unk2);
            }

            public override CDataCycleContentsUnk Read(IBuffer buffer)
            {
                CDataCycleContentsUnk obj = new CDataCycleContentsUnk();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadInt64(buffer);
                obj.Unk2 = ReadInt64(buffer);
                return obj;
            }
        }
    }

}
