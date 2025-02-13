using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSelectItemInfo
    {
        public uint Index { get; set; }
        public byte Num { get; set; }

        public class Serializer : EntitySerializer<CDataSelectItemInfo>
        {
            public override void Write(IBuffer buffer, CDataSelectItemInfo obj)
            {
                WriteUInt32(buffer, obj.Index);
                WriteByte(buffer, obj.Num);
            }

            public override CDataSelectItemInfo Read(IBuffer buffer)
            {
                CDataSelectItemInfo obj = new CDataSelectItemInfo();
                obj.Index = ReadUInt32(buffer);
                obj.Num = ReadByte(buffer);
                return obj;
            }
        }
    }
}
