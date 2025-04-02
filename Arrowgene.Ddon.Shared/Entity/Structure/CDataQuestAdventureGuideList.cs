using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestAdventureGuideList
    {
        public CDataQuestAdventureGuideList()
        {
            Param = new CDataQuestOrderList();
        }

        /// <summary>
        /// 1-8. See QUEST_MSG_ADV_GUIDE_CATEGORY strings.
        /// </summary>
        public QuestAdventureGuideCategory Category { get; set; }
        public CDataQuestOrderList Param { get; set; }
        public bool Important { get; set; }
        public bool Unk2 { get; set; }
        public uint QuestOrderBackgroundImage { get; set; }

        public class Serializer : EntitySerializer<CDataQuestAdventureGuideList>
        {
            public override void Write(IBuffer buffer, CDataQuestAdventureGuideList obj)
            {
                WriteByte(buffer, (byte) obj.Category);
                WriteEntity<CDataQuestOrderList>(buffer, obj.Param);
                WriteBool(buffer, obj.Important);
                WriteBool(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.QuestOrderBackgroundImage);
            }

            public override CDataQuestAdventureGuideList Read(IBuffer buffer)
            {
                CDataQuestAdventureGuideList obj = new CDataQuestAdventureGuideList();
                obj.Category = (QuestAdventureGuideCategory) ReadByte(buffer);
                obj.Param = ReadEntity<CDataQuestOrderList>(buffer);
                obj.Important = ReadBool(buffer);
                obj.Unk2 = ReadBool(buffer);
                obj.QuestOrderBackgroundImage = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
