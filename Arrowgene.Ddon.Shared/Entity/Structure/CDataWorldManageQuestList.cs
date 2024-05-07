using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWorldManageQuestList
    {
        public CDataWorldManageQuestList() {
            Param = new CDataQuestList();
        }
    
        public CDataQuestList Param { get; set; }
        public byte IsTutorialGuide { get; set; }
    
        public class Serializer : EntitySerializer<CDataWorldManageQuestList>
        {
            public override void Write(IBuffer buffer, CDataWorldManageQuestList obj)
            {
                WriteEntity<CDataQuestList>(buffer, obj.Param);
                WriteByte(buffer, obj.IsTutorialGuide);
            }
        
            public override CDataWorldManageQuestList Read(IBuffer buffer)
            {
                CDataWorldManageQuestList obj = new CDataWorldManageQuestList();
                obj.Param = ReadEntity<CDataQuestList>(buffer);
                obj.IsTutorialGuide = ReadByte(buffer);
                return obj;
            }
        }
    }
}