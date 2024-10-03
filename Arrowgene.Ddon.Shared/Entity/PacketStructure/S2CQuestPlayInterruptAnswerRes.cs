using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayInterruptAnswerRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_PLAY_INTERRUPT_ANSWER_RES;

        public S2CQuestPlayInterruptAnswerRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayInterruptAnswerRes>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayInterruptAnswerRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestPlayInterruptAnswerRes Read(IBuffer buffer)
            {
                S2CQuestPlayInterruptAnswerRes obj = new S2CQuestPlayInterruptAnswerRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
