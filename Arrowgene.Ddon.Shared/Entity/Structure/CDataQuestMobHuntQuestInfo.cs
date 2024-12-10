using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestMobHuntQuestInfo
    {
        public CDataQuestMobHuntQuestInfo()
        {
            QuestList = new CDataQuestList();
        }

        public CDataQuestList QuestList {  get; set; }
        public uint QuestOrderBackgroundImage { get; set; } // Dragon's Dogma Online\nativePC\rom\ui\quest\mhxxxx_id.arc
        public byte Unk0 { get; set; } // AreaId?

        public class Serializer : EntitySerializer<CDataQuestMobHuntQuestInfo>
        {
            public override void Write(IBuffer buffer, CDataQuestMobHuntQuestInfo obj)
            {
                WriteEntity(buffer, obj.QuestList);
                WriteUInt32(buffer, obj.QuestOrderBackgroundImage);
                WriteByte(buffer, obj.Unk0);
            }

            public override CDataQuestMobHuntQuestInfo Read(IBuffer buffer)
            {
                CDataQuestMobHuntQuestInfo obj = new CDataQuestMobHuntQuestInfo();
                obj.QuestList = ReadEntity<CDataQuestList>(buffer);
                obj.QuestOrderBackgroundImage = ReadUInt32(buffer);
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
