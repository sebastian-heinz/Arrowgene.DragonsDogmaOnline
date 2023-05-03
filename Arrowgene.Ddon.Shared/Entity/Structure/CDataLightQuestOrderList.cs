using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLightQuestOrderList
    {
        public CDataLightQuestOrderList() {
            Param = new CDataQuestOrderList();
            Detail = new CDataLightQuestDetail();
        }
    
        public CDataQuestOrderList Param { get; set; }
        public CDataLightQuestDetail Detail { get; set; }
    
        public class Serializer : EntitySerializer<CDataLightQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataLightQuestOrderList obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
                WriteEntity<CDataLightQuestDetail>(buffer, obj.Detail);
            }
        
            public override CDataLightQuestOrderList Read(IBuffer buffer)
            {
                CDataLightQuestOrderList obj = new CDataLightQuestOrderList();
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                obj.Detail = ReadEntity<CDataLightQuestDetail>(buffer);
                return obj;
            }
        }
    }
}