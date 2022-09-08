using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMasterInfo
    {
        public CDataMasterInfo()
        {
            UniqueId=0;
            Unk0=0;
        }

        public ulong UniqueId { get; set; } // Context's Unique ID
        public sbyte Unk0 { get; set; }

        // Could the casting be troublesome?
        public class Serializer : EntitySerializer<CDataMasterInfo>
        {
            public override void Write(IBuffer buffer, CDataMasterInfo obj)
            {
                WriteUInt64(buffer, obj.UniqueId);
                WriteByte(buffer, (byte) obj.Unk0);
            }

            public override CDataMasterInfo Read(IBuffer buffer)
            {
                CDataMasterInfo obj = new CDataMasterInfo();
                obj.UniqueId = ReadUInt64(buffer);
                obj.Unk0 = (sbyte) ReadByte(buffer);
                return obj;
            }
        }
    }
}
