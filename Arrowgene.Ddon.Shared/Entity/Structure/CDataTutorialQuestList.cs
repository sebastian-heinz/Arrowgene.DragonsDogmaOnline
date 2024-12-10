using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTutorialQuestList
    {
        public CDataTutorialQuestList()
        {
            Param = new CDataQuestList();
        }

        public CDataQuestList Param { get; set; }
        public bool EnableCancel { get; set; }

        public class Serializer : EntitySerializer<CDataTutorialQuestList>
        {
            public override void Write(IBuffer buffer, CDataTutorialQuestList obj)
            {
                WriteEntity(buffer, obj.Param);
                WriteBool(buffer, obj.EnableCancel);
            }

            public override CDataTutorialQuestList Read(IBuffer buffer)
            {
                CDataTutorialQuestList obj = new CDataTutorialQuestList();
                obj.Param = ReadEntity<CDataQuestList>(buffer);
                obj.EnableCancel = ReadBool(buffer);
                return obj;
            }
        }
    }
}
