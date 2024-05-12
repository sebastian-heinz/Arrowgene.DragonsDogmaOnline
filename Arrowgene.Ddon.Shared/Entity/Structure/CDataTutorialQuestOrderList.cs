using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTutorialQuestOrderList
    {
        public CDataTutorialQuestOrderList() {
            Param = new CDataQuestOrderList();
        }
    
        public CDataQuestOrderList Param { get; set; }
        public byte EnableCancel { get; set; }
    
        public class Serializer : EntitySerializer<CDataTutorialQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataTutorialQuestOrderList obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
                WriteByte(buffer, obj.EnableCancel);
            }
        
            public override CDataTutorialQuestOrderList Read(IBuffer buffer)
            {
                CDataTutorialQuestOrderList obj = new CDataTutorialQuestOrderList();
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                obj.EnableCancel = ReadByte(buffer);
                return obj;
            }
        }
    }
}