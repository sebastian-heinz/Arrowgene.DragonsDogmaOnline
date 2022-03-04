using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestLayoutFlag
{
    public CDataQuestLayoutFlag()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestLayoutFlag>
    {
        public override void Write(IBuffer buffer, CDataQuestLayoutFlag obj)
        {

        }

        public override CDataQuestLayoutFlag Read(IBuffer buffer)
        {
            CDataQuestLayoutFlag obj = new CDataQuestLayoutFlag();
            return obj;
        }
    }
}
