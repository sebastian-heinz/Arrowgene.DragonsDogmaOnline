using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsUnk1
    {
        public uint Unk0 { get; set; } // index (for commands 215-218)
        public uint Unk1 { get; set; } // value (for commands 217-218)

        public class Serializer : EntitySerializer<CDataCycleContentsUnk1>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsUnk1 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataCycleContentsUnk1 Read(IBuffer buffer)
            {
                CDataCycleContentsUnk1 obj = new CDataCycleContentsUnk1();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
