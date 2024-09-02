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
        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataQuestMobHuntQuestInfo>
        {
            public override void Write(IBuffer buffer, CDataQuestMobHuntQuestInfo obj)
            {
                WriteEntity(buffer, obj.QuestList);
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override CDataQuestMobHuntQuestInfo Read(IBuffer buffer)
            {
                CDataQuestMobHuntQuestInfo obj = new CDataQuestMobHuntQuestInfo();
                obj.QuestList = ReadEntity<CDataQuestList>(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
