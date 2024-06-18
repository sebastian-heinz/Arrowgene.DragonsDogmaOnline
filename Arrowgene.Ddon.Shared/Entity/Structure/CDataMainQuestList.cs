using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMainQuestList
    {
        public CDataMainQuestList()
        {
            Param = new CDataQuestList();
        }

        public CDataQuestList Param { get; set; }

        public class Serializer : EntitySerializer<CDataMainQuestList>
        {
            public override void Write(IBuffer buffer, CDataMainQuestList obj)
            {
                WriteEntity<CDataQuestList>(buffer, obj.Param);
            }

            public override CDataMainQuestList Read(IBuffer buffer)
            {
                CDataMainQuestList obj = new CDataMainQuestList();
                obj.Param = ReadEntity<CDataQuestList>(buffer);
                return obj;
            }
        }
    }
}
