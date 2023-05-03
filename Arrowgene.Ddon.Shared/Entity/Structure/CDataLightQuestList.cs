using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLightQuestList
    {
        public CDataLightQuestList() {
            Param = new CDataQuestList();
            Detail = new CDataLightQuestDetail();
            Contents = new CDataQuestContents();
        }
    
        public CDataQuestList Param { get; set; }
        public CDataLightQuestDetail Detail  { get; set; }
        public CDataQuestContents Contents { get; set; }
    
        public class Serializer : EntitySerializer<CDataLightQuestList>
        {
            public override void Write(IBuffer buffer, CDataLightQuestList obj)
            {
                WriteEntity<CDataQuestList>(buffer, obj.Param);
                WriteEntity<CDataLightQuestDetail>(buffer, obj.Detail);
                WriteEntity<CDataQuestContents>(buffer, obj.Contents);
            }
        
            public override CDataLightQuestList Read(IBuffer buffer)
            {
                CDataLightQuestList obj = new CDataLightQuestList();
                obj.Param = ReadEntity<CDataQuestList>(buffer);
                obj.Detail = ReadEntity<CDataLightQuestDetail>(buffer);
                obj.Contents = ReadEntity<CDataQuestContents>(buffer);
                return obj;
            }
        }
    }
}