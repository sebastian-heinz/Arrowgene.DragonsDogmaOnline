using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestCompleteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_11_27_16_NTC;

        // public PacketId Id => PacketId.S2C_QUEST_11_7_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_20_16_NTC; // Nothing happened


        // public PacketId Id => PacketId.S2C_QUEST_11_25_16_NTC; // S2C_QUEST_CANCEL_NOTICE?
        // public PacketId Id => PacketId.S2C_QUEST_11_26_16_NTC;    // S2C_QUEST_ENABLE_NOTICE?
        // public PacketId Id => PacketId.S2C_QUEST_11_27_16_NTC; // Nothing happened

        // public PacketId Id => PacketId.S2C_QUEST_11_32_16_NTC;
        // public PacketId Id => PacketId.S2C_QUEST_11_40_16_NTC; // Seems to do nothing
        // public PacketId Id => PacketId.S2C_QUEST_11_47_16_NTC; // <stslog_quest_progress_failed>

        // public PacketId Id => PacketId.S2C_QUEST_11_68_16_NTC; // Special Quest Board Reloaded
        // public PacketId Id => PacketId.S2C_QUEST_11_69_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_70_16_NTC;
        // public PacketId Id => PacketId.S2C_QUEST_11_73_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_81_16_NTC; // <syslog_quest_progress_failed>
        // public PacketId Id => PacketId.S2C_QUEST_11_82_16_NTC;
        // public PacketId Id => PacketId.S2C_QUEST_11_83_16_NTC;
        // public PacketId Id => PacketId.S2C_QUEST_11_84_16_NTC; // Crashed game
        // public PacketId Id => PacketId.S2C_QUEST_11_85_16_NTC; // nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_86_16_NTC; // Nothing happened

        // public PacketId Id => PacketId.S2C_QUEST_11_87_16_NTC; // Probably S2C_QUEST_MASTER_DATA_RELOAD_NOTICE


        // public PacketId Id => PacketId.S2C_QUEST_11_90_16_NTC; // nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_91_16_NTC; // Mission Accepted
        // public PacketId Id => PacketId.S2C_QUEST_11_92_16_NTC; // Mission Started
        // public PacketId Id => PacketId.S2C_QUEST_11_93_16_NTC; // Mission completed
        // public PacketId Id => PacketId.S2C_QUEST_11_94_16_NTC; // Mission  All completed
        // public PacketId Id => PacketId.S2C_QUEST_11_95_16_NTC; // Crashed Game
        // public PacketId Id => PacketId.S2C_QUEST_11_96_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_97_16_NTC; // Crashed Game
        // public PacketId Id => PacketId.S2C_QUEST_11_98_16_NTC; // Crashed Game
        // public PacketId Id => PacketId.S2C_QUEST_11_99_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_100_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_101_16_NTC; // <syslog_quest_progress_failed>
        // public PacketId Id => PacketId.S2C_QUEST_11_102_16_NTC; // Remaining time extended by 0 seconds
        // public PacketId Id => PacketId.S2C_QUEST_11_103_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_104_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_105_16_NTC; // Time is up
        // public PacketId Id => PacketId.S2C_QUEST_11_106_16_NTC; // <syslog_wm_statement_gauge_0_defeat>

        // public PacketId Id => PacketId.S2C_QUEST_11_107_16_NTC; // The request to end the mission was successful
        // public PacketId Id => PacketId.S2C_QUEST_11_108_16_NTC; // The request to end the mission was successful

        // public PacketId Id => PacketId.S2C_QUEST_11_109_16_NTC; // Nothing happened

        // public PacketId Id => PacketId.S2C_QUEST_11_110_16_NTC; // <syslog_quest_progress_failed>
        // public PacketId Id => PacketId.S2C_QUEST_11_111_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_112_16_NTC; // Nothing Happened
        // public PacketId Id => PacketId.S2C_QUEST_11_113_16_NTC; // nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_114_16_NTC; // <syslog_quest_progress_failed>
        // public PacketId Id => PacketId.S2C_QUEST_11_115_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_116_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_117_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_118_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_119_16_NTC; // Nothing happened
        // public PacketId Id => PacketId.S2C_QUEST_11_124_16_NTC; // <syslog_quest_progress_failed>
        // public PacketId Id => PacketId.S2C_QUEST_11_125_16_NTC; // Nothing happened

        // public PacketId Id => PacketId.S2C_QUEST_11_126_16_NTC; // Nothing happened



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
