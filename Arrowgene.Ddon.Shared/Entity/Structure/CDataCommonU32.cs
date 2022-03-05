using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCommonU32
    {
        public CDataCommonU32(uint value)
        {
            Value = value;
        }

        public CDataCommonU32()
        {
            Value=0;
        }

        public uint Value { get; set; }

        public class Serializer : EntitySerializer<CDataCommonU32>
        {
            public override void Write(IBuffer buffer, CDataCommonU32 obj)
            {
                WriteUInt32(buffer, obj.Value);
            }

            public override CDataCommonU32 Read(IBuffer buffer)
            {
                CDataCommonU32 obj = new CDataCommonU32();
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
