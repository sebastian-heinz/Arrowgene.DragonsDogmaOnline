using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanValueInfo
    {
        public byte Type { get; set; }
        public uint Value { get; set; }

        public class Serializer : EntitySerializer<CDataClanValueInfo>
        {
            public override void Write(IBuffer buffer, CDataClanValueInfo obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.Value);
            }

            public override CDataClanValueInfo Read(IBuffer buffer)
            {
                CDataClanValueInfo obj = new CDataClanValueInfo();
                obj.Type = ReadByte(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
