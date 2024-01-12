using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLotQuestOrderList
    {
        public CDataLotQuestOrderList() {
            Param = new CDataQuestOrderList();
        }
    
        public CDataQuestOrderList Param { get; set; }
        public byte LotQuestType { get; set; }
    
        public class Serializer : EntitySerializer<CDataLotQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataLotQuestOrderList obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
                WriteByte(buffer, obj.LotQuestType);
            }
        
            public override CDataLotQuestOrderList Read(IBuffer buffer)
            {
                CDataLotQuestOrderList obj = new CDataLotQuestOrderList();
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                obj.LotQuestType = ReadByte(buffer);
                return obj;
            }
        }
    }
}