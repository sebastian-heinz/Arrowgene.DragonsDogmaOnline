using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestFlag
{
    public CDataQuestFlag()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestFlag>
    {
        public override void Write(IBuffer buffer, CDataQuestFlag obj)
        {

        }

        public override CDataQuestFlag Read(IBuffer buffer)
        {
            CDataQuestFlag obj = new CDataQuestFlag();
            return obj;
        }
    }
}
