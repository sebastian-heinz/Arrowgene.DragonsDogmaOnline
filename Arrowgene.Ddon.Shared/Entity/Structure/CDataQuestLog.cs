using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestLog
    {
        public CDataQuestLog() {
            QuestAnnounceList = new List<CDataQuestAnnounce>();
            QuestTalkInfoList = new List<CDataQuestTalkInfo>();
        }
    
        public List<CDataQuestAnnounce> QuestAnnounceList;
        public List<CDataQuestTalkInfo> QuestTalkInfoList;
    
        public class Serializer : EntitySerializer<CDataQuestLog>
        {
            public override void Write(IBuffer buffer, CDataQuestLog obj)
            {
                WriteEntityList<CDataQuestAnnounce>(buffer, obj.QuestAnnounceList);
                WriteEntityList<CDataQuestTalkInfo>(buffer, obj.QuestTalkInfoList);
            }
        
            public override CDataQuestLog Read(IBuffer buffer)
            {
                CDataQuestLog obj = new CDataQuestLog();
                obj.QuestAnnounceList = ReadEntityList<CDataQuestAnnounce>(buffer);
                obj.QuestTalkInfoList = ReadEntityList<CDataQuestTalkInfo>(buffer);
                return obj;
            }
        }
    }
}