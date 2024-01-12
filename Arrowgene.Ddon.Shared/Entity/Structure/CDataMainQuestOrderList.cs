using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMainQuestOrderList
    {
        public CDataMainQuestOrderList() {
            Param = new CDataQuestOrderList();
        }
    
        public CDataQuestOrderList Param { get; set; }
    
        public class Serializer : EntitySerializer<CDataMainQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataMainQuestOrderList obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
            }
        
            public override CDataMainQuestOrderList Read(IBuffer buffer)
            {
                CDataMainQuestOrderList obj = new CDataMainQuestOrderList();
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                return obj;
            }
        }
    }
}