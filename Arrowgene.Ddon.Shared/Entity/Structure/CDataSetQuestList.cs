using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSetQuestList
    {
        public CDataSetQuestList() {
            Param = new CDataQuestList();
            Detail = new CDataSetQuestDetail();
        }
    
        public CDataQuestList Param { get; set; }
        public CDataSetQuestDetail Detail { get; set; }
    
        public class Serializer : EntitySerializer<CDataSetQuestList>
        {
            public override void Write(IBuffer buffer, CDataSetQuestList obj)
            {
                WriteEntity<CDataQuestList>(buffer, obj.Param);
                WriteEntity<CDataSetQuestDetail>(buffer, obj.Detail);
            }
        
            public override CDataSetQuestList Read(IBuffer buffer)
            {
                CDataSetQuestList obj = new CDataSetQuestList();
                obj.Param = ReadEntity<CDataQuestList>(buffer);
                obj.Detail = ReadEntity<CDataSetQuestDetail>(buffer);
                return obj;
            }
        }
    }
}