using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestTalkInfo
{
    public CDataQuestTalkInfo()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestTalkInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestTalkInfo obj)
        {

        }

        public override CDataQuestTalkInfo Read(IBuffer buffer)
        {
            CDataQuestTalkInfo obj = new CDataQuestTalkInfo();
            return obj;
        }
    }
}
