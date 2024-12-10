using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayEntryCancelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_PLAY_ENTRY_CANCEL_RES;

        public S2CQuestPlayEntryCancelRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayEntryCancelRes>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayEntryCancelRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestPlayEntryCancelRes Read(IBuffer buffer)
            {
                S2CQuestPlayEntryCancelRes obj = new S2CQuestPlayEntryCancelRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
