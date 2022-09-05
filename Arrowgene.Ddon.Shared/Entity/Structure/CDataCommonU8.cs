using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCommonU8
    {
        public CDataCommonU8()
        {
            Value = 0;
        }

        public CDataCommonU8(byte value)
        {
            Value = value;
        }

        public byte Value { get; set; }

        public class Serializer : EntitySerializer<CDataCommonU8>
        {
            public override void Write(IBuffer buffer, CDataCommonU8 obj)
            {
                WriteByte(buffer, obj.Value);
            }

            public override CDataCommonU8 Read(IBuffer buffer)
            {
                CDataCommonU8 obj = new CDataCommonU8();
                obj.Value = ReadByte(buffer);
                return obj;
            }
        }
    }
}