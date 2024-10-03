using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayAddTimerNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_ADD_TIMER_NTC;

        public S2CQuestPlayAddTimerNtc()
        {
        }

        public ulong PlayEndDateTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayAddTimerNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayAddTimerNtc obj)
            {
                WriteUInt64(buffer, obj.PlayEndDateTime);
            }

            public override S2CQuestPlayAddTimerNtc Read(IBuffer buffer)
            {
                S2CQuestPlayAddTimerNtc obj = new S2CQuestPlayAddTimerNtc();
                obj.PlayEndDateTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
