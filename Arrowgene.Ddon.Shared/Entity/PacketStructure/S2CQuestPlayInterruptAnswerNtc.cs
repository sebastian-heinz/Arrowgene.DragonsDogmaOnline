using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayInterruptAnswerNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_INTERRUPT_RESULT_NTC;

        public S2CQuestPlayInterruptAnswerNtc()
        {
        }

        public bool IsInterrupt {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayInterruptAnswerNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayInterruptAnswerNtc obj)
            {
                WriteBool(buffer, obj.IsInterrupt);
            }

            public override S2CQuestPlayInterruptAnswerNtc Read(IBuffer buffer)
            {
                S2CQuestPlayInterruptAnswerNtc obj = new S2CQuestPlayInterruptAnswerNtc();
                obj.IsInterrupt = ReadBool(buffer);
                return obj;
            }
        }
    }
}
