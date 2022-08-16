using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOrderConditionInfo
    {
        public uint QuestScheduleId { get; set; }
        public bool CanProgress { get; set; }

        public class Serializer : EntitySerializer<CDataOrderConditionInfo>
        {
            public override void Write(IBuffer buffer, CDataOrderConditionInfo entity)
            {
                WriteUInt32(buffer, entity.QuestScheduleId);
                WriteBool(buffer, entity.CanProgress);
            }

            public override CDataOrderConditionInfo Read(IBuffer buffer)
            {
                CDataOrderConditionInfo entity = new CDataOrderConditionInfo();
                entity.QuestScheduleId = ReadUInt32(buffer);
                entity.CanProgress = ReadBool(buffer);
                return entity;
            }
        }
    }
}