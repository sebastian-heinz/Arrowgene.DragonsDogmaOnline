using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMobHuntQuestDetail
    {
        public uint QuestOrderBackgroundImage { get; set; }
        public byte Unk0 { get; set; }

        public class Serializer : EntitySerializer<CDataMobHuntQuestDetail>
        {
            public override void Write(IBuffer buffer, CDataMobHuntQuestDetail obj)
            {
                WriteUInt32(buffer, obj.QuestOrderBackgroundImage);
                WriteByte(buffer, obj.Unk0);
            }

            public override CDataMobHuntQuestDetail Read(IBuffer buffer)
            {
                CDataMobHuntQuestDetail obj = new CDataMobHuntQuestDetail
                {
                    QuestOrderBackgroundImage = ReadUInt32(buffer),
                    Unk0 = ReadByte(buffer)
                };
                return obj;
            }
        }
    }
}
