using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSetQuestOrderList
    {
        public CDataSetQuestOrderList() {
            Param = new CDataQuestOrderList();
            Detail = new CDataSetQuestDetail();
        }
    
        public CDataQuestOrderList Param { get; set; }
        public CDataSetQuestDetail Detail { get; set; }
    
        public class Serializer : EntitySerializer<CDataSetQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataSetQuestOrderList obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
                WriteEntity<CDataSetQuestDetail>(buffer, obj.Detail);
            }
        
            public override CDataSetQuestOrderList Read(IBuffer buffer)
            {
                CDataSetQuestOrderList obj = new CDataSetQuestOrderList();
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                obj.Detail = ReadEntity<CDataSetQuestDetail>(buffer);
                return obj;
            }
        }
    }
}