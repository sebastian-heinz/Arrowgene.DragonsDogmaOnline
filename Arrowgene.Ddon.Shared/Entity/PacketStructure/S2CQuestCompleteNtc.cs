using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestCompleteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_QUEST_COMPLETE_NTC;

        public S2CQuestCompleteNtc()
        {
        }

        public UInt32 QuestScheduleId {  get; set; }
        public byte RandomRewardNum {  get; set; }
        public byte ChargeRewardNum {  get; set; } // Multiple boxs with ????
        public byte ProgressBonusNum { get; set; } // Get ??? to show up in UI
        public bool IsRepeatReward {  get; set; } // Repeat reward
        public bool IsUndiscoveredReward {  get; set; }  // Earned a hidden reward
        public bool IsHelpReward {  get; set; }
        public bool IsPartyBonus {  get; set; }
        
        public class Serializer : PacketEntitySerializer<S2CQuestCompleteNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestCompleteNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteByte(buffer, obj.RandomRewardNum);
                WriteByte(buffer, obj.ChargeRewardNum);
                WriteByte(buffer, obj.ProgressBonusNum);
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
                obj.ProgressBonusNum = ReadByte(buffer);
                obj.IsRepeatReward = ReadBool(buffer);
                obj.IsUndiscoveredReward = ReadBool(buffer);
                obj.IsHelpReward = ReadBool(buffer);
                obj.IsPartyBonus = ReadBool(buffer);
                return obj;
            }
        }
    }
}
