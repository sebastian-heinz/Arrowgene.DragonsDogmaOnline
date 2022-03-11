using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestIdScheduleId
{
    public CDataQuestIdScheduleId()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestIdScheduleId>
    {
        public override void Write(IBuffer buffer, CDataQuestIdScheduleId obj)
        {

        }

        public override CDataQuestIdScheduleId Read(IBuffer buffer)
        {
            CDataQuestIdScheduleId obj = new CDataQuestIdScheduleId();
            return obj;
        }
    }
}
