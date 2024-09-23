using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCommonU64
    {
        public CDataCommonU64(ulong value)
        {
            Value = value;
        }

        public CDataCommonU64()
        {
            Value = 0;
        }

        public ulong Value { get; set; }

        public class Serializer : EntitySerializer<CDataCommonU64>
        {
            public override void Write(IBuffer buffer, CDataCommonU64 obj)
            {
                WriteUInt64(buffer, obj.Value);
            }

            public override CDataCommonU64 Read(IBuffer buffer)
            {
                CDataCommonU64 obj = new CDataCommonU64();
                obj.Value = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}

