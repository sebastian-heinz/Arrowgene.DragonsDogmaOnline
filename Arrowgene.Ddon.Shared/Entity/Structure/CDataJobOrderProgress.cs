using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobOrderProgress
    {
        public JobOrderCondType ConditionType { get; set; }
        public uint TargetId { get; set; }
        public uint TargetRank { get; set; }
        public uint TargetNum { get; set; }
        public uint CurrentNum { get; set; }

        public class Serializer : EntitySerializer<CDataJobOrderProgress>
        {
            public override void Write(IBuffer buffer, CDataJobOrderProgress obj)
            {
                WriteByte(buffer, (byte) obj.ConditionType);
                WriteUInt32(buffer, obj.TargetId);
                WriteUInt32(buffer, obj.TargetRank);
                WriteUInt32(buffer, obj.TargetNum);
                WriteUInt32(buffer, obj.CurrentNum);
            }

            public override CDataJobOrderProgress Read(IBuffer buffer)
            {
                CDataJobOrderProgress obj = new CDataJobOrderProgress();
                obj.ConditionType = (JobOrderCondType) ReadByte(buffer);
                obj.TargetId = ReadUInt32(buffer);
                obj.TargetRank = ReadUInt32(buffer);
                obj.TargetNum = ReadUInt32(buffer);
                obj.CurrentNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
