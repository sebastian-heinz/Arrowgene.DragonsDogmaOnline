using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSetQuestOrderList
    {
        public CDataSetQuestOrderList() {
            Param = new CDataQuestOrderList();
            Detail = new CDataSetQuestDetail();
        }
    
        public QuestAreaId AreaId { get; set; }
        public CDataQuestOrderList Param { get; set; }
        public CDataSetQuestDetail Detail { get; set; }
    
        public class Serializer : EntitySerializer<CDataSetQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataSetQuestOrderList obj)
            {
                WriteUInt32(buffer, (uint) obj.AreaId);
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
                WriteEntity<CDataSetQuestDetail>(buffer, obj.Detail);
            }
        
            public override CDataSetQuestOrderList Read(IBuffer buffer)
            {
                CDataSetQuestOrderList obj = new CDataSetQuestOrderList();
                obj.AreaId = (QuestAreaId)ReadUInt32(buffer);
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                obj.Detail = ReadEntity<CDataSetQuestDetail>(buffer);
                return obj;
            }
        }
    }
}
