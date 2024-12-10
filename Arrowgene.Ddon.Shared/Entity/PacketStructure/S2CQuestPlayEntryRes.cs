using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayEntryRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_PLAY_ENTRY_RES;

        public S2CQuestPlayEntryRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayEntryRes>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayEntryRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestPlayEntryRes Read(IBuffer buffer)
            {
                S2CQuestPlayEntryRes obj = new S2CQuestPlayEntryRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
