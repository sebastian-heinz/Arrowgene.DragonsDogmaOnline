using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMasterInfo
    {
        public CDataMasterInfo()
        {
            UniqueId=0;
            MasterIndex=0;
        }

        public ulong UniqueId { get; set; } // Context's Unique ID
        public sbyte MasterIndex { get; set; }

        // Could the casting be troublesome?
        public class Serializer : EntitySerializer<CDataMasterInfo>
        {
            public override void Write(IBuffer buffer, CDataMasterInfo obj)
            {
                WriteUInt64(buffer, obj.UniqueId);
                WriteByte(buffer, (byte) obj.MasterIndex);
            }

            public override CDataMasterInfo Read(IBuffer buffer)
            {
                CDataMasterInfo obj = new CDataMasterInfo();
                obj.UniqueId = ReadUInt64(buffer);
                obj.MasterIndex = (sbyte) ReadByte(buffer);
                return obj;
            }
        }
    }
}
