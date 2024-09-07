using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayStartTimerNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_START_TIMER_NTC;

        public S2CQuestPlayStartTimerNtc()
        {
        }

        public ulong PlayEndDateTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayStartTimerNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayStartTimerNtc obj)
            {
                WriteUInt64(buffer, obj.PlayEndDateTime);
            }

            public override S2CQuestPlayStartTimerNtc Read(IBuffer buffer)
            {
                S2CQuestPlayStartTimerNtc obj = new S2CQuestPlayStartTimerNtc();
                obj.PlayEndDateTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
