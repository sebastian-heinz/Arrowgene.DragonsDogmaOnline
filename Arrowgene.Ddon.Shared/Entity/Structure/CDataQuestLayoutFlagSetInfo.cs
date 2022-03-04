using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestLayoutFlagSetInfo
{
    public CDataQuestLayoutFlagSetInfo()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestLayoutFlagSetInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestLayoutFlagSetInfo obj)
        {

        }

        public override CDataQuestLayoutFlagSetInfo Read(IBuffer buffer)
        {
            CDataQuestLayoutFlagSetInfo obj = new CDataQuestLayoutFlagSetInfo();
            return obj;
        }
    }
}
