using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaBonus
    {
        // Shorts are likely
        // u16 m_usGoldRatio;
        // u16 m_usExpRatio;
        // u16 m_usRimRatio;
        // u16 m_usAreaPointRatio;
        // plus one more from S3.

        public QuestAreaId AreaID { get; set; }
        public ushort Unk0 { get; set; }
        public ushort Unk1 { get; set; }
        public ushort Unk2 { get; set; }
        public ushort Unk3 { get; set; }
        public ushort Unk4 { get; set; }


        public class Serializer : EntitySerializer<CDataAreaBonus>
        {
            public override void Write(IBuffer buffer, CDataAreaBonus obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaID);
                WriteUInt16(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
                WriteUInt16(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
                WriteUInt16(buffer, obj.Unk4);
            }

            public override CDataAreaBonus Read(IBuffer buffer)
            {
                CDataAreaBonus obj = new CDataAreaBonus();
                obj.AreaID = (QuestAreaId)ReadUInt32(buffer);
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                obj.Unk2 = ReadUInt16(buffer);
                obj.Unk3 = ReadUInt16(buffer);
                obj.Unk4 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
