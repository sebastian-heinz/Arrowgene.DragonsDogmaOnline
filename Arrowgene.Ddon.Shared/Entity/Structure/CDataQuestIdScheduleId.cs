using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestIdScheduleId
{
    public uint QuestId { get; set; }
    public uint QuestScheduleId { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestIdScheduleId>
    {
        public override void Write(IBuffer buffer, CDataQuestIdScheduleId obj)
        {
            WriteUInt32(buffer, obj.QuestId);
            WriteUInt32(buffer, obj.QuestScheduleId);
        }

        public override CDataQuestIdScheduleId Read(IBuffer buffer)
        {
            CDataQuestIdScheduleId obj = new CDataQuestIdScheduleId();
            obj.QuestId = ReadUInt32(buffer);
            obj.QuestScheduleId = ReadUInt32(buffer);
            return obj;
        }
    }
}
