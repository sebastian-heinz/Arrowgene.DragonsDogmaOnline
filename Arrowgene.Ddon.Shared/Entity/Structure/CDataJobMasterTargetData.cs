using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobMasterTargetData
    {
        public ReleaseType ReleaseType { get; set; }
        public uint ReleaseId { get; set; }
        public byte ReleaseLv { get; set; }
        public JobOrderCondType Condition { get; set; }
        public uint TargetId { get; set; }
        public uint TargetRank { get; set; }

        public class Serializer : EntitySerializer<CDataJobMasterTargetData>
        {
            public override void Write(IBuffer buffer, CDataJobMasterTargetData obj)
            {
                WriteByte(buffer, (byte)obj.ReleaseType);
                WriteUInt32(buffer, obj.ReleaseId);
                WriteByte(buffer, obj.ReleaseLv);
                WriteByte(buffer, (byte) obj.Condition);
                WriteUInt32(buffer, obj.TargetId);
                WriteUInt32(buffer, obj.TargetRank);
            }

            public override CDataJobMasterTargetData Read(IBuffer buffer)
            {
                CDataJobMasterTargetData obj = new CDataJobMasterTargetData();
                obj.ReleaseType = (ReleaseType)ReadByte(buffer);
                obj.ReleaseId = ReadUInt32(buffer);
                obj.ReleaseLv = ReadByte(buffer);
                obj.Condition = (JobOrderCondType) ReadByte(buffer);
                obj.TargetId = ReadUInt32(buffer);
                obj.TargetRank = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
