using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWorldManageQuestOrderList
    {
        public CDataWorldManageQuestOrderList() {
            Param = new CDataQuestOrderList();
        }
    
        public CDataQuestOrderList Param { get; set; }
        public byte IsTutorialGuide { get; set; }
    
        public class Serializer : EntitySerializer<CDataWorldManageQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataWorldManageQuestOrderList obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
                WriteByte(buffer, obj.IsTutorialGuide);
            }
        
            public override CDataWorldManageQuestOrderList Read(IBuffer buffer)
            {
                CDataWorldManageQuestOrderList obj = new CDataWorldManageQuestOrderList();
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                obj.IsTutorialGuide = ReadByte(buffer);
                return obj;
            }
        }
    }
}