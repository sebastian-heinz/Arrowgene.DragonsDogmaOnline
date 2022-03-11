using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestProcessState
{
    public CDataQuestProcessState()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestProcessState>
    {
        public override void Write(IBuffer buffer, CDataQuestProcessState obj)
        {

        }

        public override CDataQuestProcessState Read(IBuffer buffer)
        {
            CDataQuestProcessState obj = new CDataQuestProcessState();
            return obj;
        }
    }
}
