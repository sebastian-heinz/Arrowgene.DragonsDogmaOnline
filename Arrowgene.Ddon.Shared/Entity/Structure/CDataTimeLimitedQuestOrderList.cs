using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTimeLimitedQuestOrderList
    {
        public CDataTimeLimitedQuestOrderList() {
            Param = new CDataQuestOrderList();
        }
    
        public CDataQuestOrderList Param { get; set; }
    
        public class Serializer : EntitySerializer<CDataTimeLimitedQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataTimeLimitedQuestOrderList obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
            }
        
            public override CDataTimeLimitedQuestOrderList Read(IBuffer buffer)
            {
                CDataTimeLimitedQuestOrderList obj = new CDataTimeLimitedQuestOrderList();
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                return obj;
            }
        }
    }
}