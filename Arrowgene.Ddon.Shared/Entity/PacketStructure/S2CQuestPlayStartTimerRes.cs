using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayStartTimerRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_PLAY_START_TIMER_RES;

        public S2CQuestPlayStartTimerRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayStartTimerRes>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayStartTimerRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestPlayStartTimerRes Read(IBuffer buffer)
            {
                S2CQuestPlayStartTimerRes obj = new S2CQuestPlayStartTimerRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}

