using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSetQuestBonusList
    {
        public CDataSetQuestBonusList()
        {
        }

        public uint Unk0 { get; set; }
        public CDataSetQuestInfoList QuestInfoList { get; set; } = new();

        public class Serializer : EntitySerializer<CDataSetQuestBonusList>
        {
            public override void Write(IBuffer buffer, CDataSetQuestBonusList obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntity<CDataSetQuestInfoList>(buffer, obj.QuestInfoList);
            }

            public override CDataSetQuestBonusList Read(IBuffer buffer)
            {
                CDataSetQuestBonusList obj = new CDataSetQuestBonusList();
                obj.Unk0 = ReadUInt32(buffer);
                obj.QuestInfoList = ReadEntity<CDataSetQuestInfoList>(buffer);

                return obj;
            }
        }
    }
}
