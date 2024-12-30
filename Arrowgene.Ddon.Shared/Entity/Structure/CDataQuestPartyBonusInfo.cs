using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestPartyBonusInfo
    {
        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }
        public ushort GoldRatio { get; set; }
        public ushort ExpRatio { get; set; }
        public ushort RimRatio { get; set; }
        public ushort AreaPointRatio { get; set; }
        public uint Dorb { get; set; }
        public bool IsReceived { get; set; }

        public class Serializer : EntitySerializer<CDataQuestPartyBonusInfo>
        {
            public override void Write(IBuffer buffer, CDataQuestPartyBonusInfo obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt16(buffer, obj.GoldRatio);
                WriteUInt16(buffer, obj.ExpRatio);
                WriteUInt16(buffer, obj.RimRatio);
                WriteUInt16(buffer, obj.AreaPointRatio);
                WriteUInt32(buffer, obj.Dorb);
                WriteBool(buffer, obj.IsReceived);
            }

            public override CDataQuestPartyBonusInfo Read(IBuffer buffer)
            {
                CDataQuestPartyBonusInfo obj = new CDataQuestPartyBonusInfo();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.GoldRatio = ReadUInt16(buffer);
                obj.ExpRatio = ReadUInt16(buffer);
                obj.RimRatio = ReadUInt16(buffer);
                obj.AreaPointRatio = ReadUInt16(buffer);
                obj.Dorb = ReadUInt32(buffer);
                obj.IsReceived = ReadBool(buffer);
                return obj;
            }
        }
    }
}
