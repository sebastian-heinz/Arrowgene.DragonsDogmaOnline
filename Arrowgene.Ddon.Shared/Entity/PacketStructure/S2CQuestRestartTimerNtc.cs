using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestRestartTimerNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_11_104_16_NTC;

        public S2CQuestRestartTimerNtc()
        {
        }

        public ulong PlayEndDateTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestRestartTimerNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestRestartTimerNtc obj)
            {
                WriteUInt64(buffer, obj.PlayEndDateTime);
            }

            public override S2CQuestRestartTimerNtc Read(IBuffer buffer)
            {
                S2CQuestRestartTimerNtc obj = new S2CQuestRestartTimerNtc();
                obj.PlayEndDateTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
