using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestStopTimerNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_11_103_16_NTC;

        public class Serializer : PacketEntitySerializer<S2CQuestStopTimerNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestStopTimerNtc obj)
            {
            }

            public override S2CQuestStopTimerNtc Read(IBuffer buffer)
            {
                return new S2CQuestStopTimerNtc();
            }
        }
    }
}
