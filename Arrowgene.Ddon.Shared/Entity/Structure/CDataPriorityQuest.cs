using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPriorityQuest
    {
        public CDataPriorityQuest()
        {
            QuestAnnounceList = new List<CDataQuestAnnounce>();
            WorkList = new List<CDataQuestProgressWork>();
        }
    
        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }
        public List<CDataQuestAnnounce> QuestAnnounceList { get; set; }
        public List<CDataQuestProgressWork> WorkList { get; set; }
    
        public class Serializer : EntitySerializer<CDataPriorityQuest>
        {
            public override void Write(IBuffer buffer, CDataPriorityQuest obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteEntityList<CDataQuestAnnounce>(buffer, obj.QuestAnnounceList);
                WriteEntityList<CDataQuestProgressWork>(buffer, obj.WorkList);
            }
        
            public override CDataPriorityQuest Read(IBuffer buffer)
            {
                CDataPriorityQuest obj = new CDataPriorityQuest();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.QuestAnnounceList = ReadEntityList<CDataQuestAnnounce>(buffer);
                obj.WorkList = ReadEntityList<CDataQuestProgressWork>(buffer);
                return obj;
            }
        }
    }
}
