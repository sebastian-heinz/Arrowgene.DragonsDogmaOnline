using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataExpiredQuestList
    {
    
        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }
    
        public class Serializer : EntitySerializer<CDataExpiredQuestList>
        {
            public override void Write(IBuffer buffer, CDataExpiredQuestList obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
            }
        
            public override CDataExpiredQuestList Read(IBuffer buffer)
            {
                CDataExpiredQuestList obj = new CDataExpiredQuestList();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}