using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestOrderConditionParam
{
    public CDataQuestOrderConditionParam()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestOrderConditionParam>
    {
        public override void Write(IBuffer buffer, CDataQuestOrderConditionParam obj)
        {

        }

        public override CDataQuestOrderConditionParam Read(IBuffer buffer)
        {
            CDataQuestOrderConditionParam obj = new CDataQuestOrderConditionParam();
            return obj;
        }
    }
}
