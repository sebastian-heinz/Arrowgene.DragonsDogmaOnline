using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCommonU16
    {
        public CDataCommonU16(ushort value)
        {
            Value = value;
        }

        public CDataCommonU16()
        {
            Value = 0;
        }

        public ushort Value { get; set; }

        public class Serializer : EntitySerializer<CDataCommonU16>
        {
            public override void Write(IBuffer buffer, CDataCommonU16 obj)
            {
                WriteUInt16(buffer, obj.Value);
            }

            public override CDataCommonU16 Read(IBuffer buffer)
            {
                CDataCommonU16 obj = new CDataCommonU16();
                obj.Value = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
