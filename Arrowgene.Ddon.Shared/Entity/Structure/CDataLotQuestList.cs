using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLotQuestList
    {
        public CDataLotQuestList()
        {
        }

        public CDataQuestList Param { get; set; } = new();
        public CDataQuestContents QuestContents { get; set; } = new();

        public class Serializer : EntitySerializer<CDataLotQuestList>
        {
            public override void Write(IBuffer buffer, CDataLotQuestList obj)
            {
                WriteEntity(buffer, obj.Param);
                WriteEntity(buffer, obj.QuestContents);
            }

            public override CDataLotQuestList Read(IBuffer buffer)
            {
                CDataLotQuestList obj = new CDataLotQuestList();
                obj.Param = ReadEntity<CDataQuestList>(buffer);
                obj.QuestContents = ReadEntity<CDataQuestContents>(buffer);
                return obj;
            }
        }
    }
}
