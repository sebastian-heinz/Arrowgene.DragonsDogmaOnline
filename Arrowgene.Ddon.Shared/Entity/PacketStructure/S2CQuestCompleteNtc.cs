using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestCompleteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_11_26_16_NTC;
        // public PacketId Id => PacketId.S2C_QUEST_11_25_16_NTC; // Cancels the quest?
        // public PacketId Id => PacketId.S2C_QUEST_11_32_16_NTC;
        // public PacketId Id => PacketId.S2C_QUEST_11_91_16_NTC;
        // public PacketId Id => PacketId.S2C_QUEST_11_7_16_NTC;

        public S2CQuestCompleteNtc()
        {
        }

        public UInt32 QuestScheduleId {  get; set; }
        public byte RandomRewardNum {  get; set; }
        public byte ChargeRewardNum {  get; set; }
        public bool IsRepeatReward {  get; set; }
        public bool IsUndiscoveredReward {  get; set; }
        public bool IsHelpReward {  get; set; }
        public bool IsPartyBonus {  get; set; }
        
        public class Serializer : PacketEntitySerializer<S2CQuestCompleteNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestCompleteNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteByte(buffer, obj.RandomRewardNum);
                WriteByte(buffer, obj.ChargeRewardNum);
                WriteBool(buffer, obj.IsRepeatReward);
                WriteBool(buffer, obj.IsUndiscoveredReward);
                WriteBool(buffer, obj.IsHelpReward);
                WriteBool(buffer, obj.IsPartyBonus);
            }

            public override S2CQuestCompleteNtc Read(IBuffer buffer)
            {
                S2CQuestCompleteNtc obj = new S2CQuestCompleteNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.RandomRewardNum = ReadByte(buffer);
                obj.ChargeRewardNum = ReadByte(buffer);
                obj.IsRepeatReward = ReadBool(buffer);
                obj.IsUndiscoveredReward = ReadBool(buffer);
                obj.IsHelpReward = ReadBool(buffer);
                obj.IsPartyBonus = ReadBool(buffer);
                return obj;
            }
        }
    }
}
