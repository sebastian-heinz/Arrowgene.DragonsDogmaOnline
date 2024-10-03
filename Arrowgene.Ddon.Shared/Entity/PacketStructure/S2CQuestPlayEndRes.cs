using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayEndRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_PLAY_END_RES;

        public S2CQuestPlayEndRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayEndRes>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayEndRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestPlayEndRes Read(IBuffer buffer)
            {
                S2CQuestPlayEndRes obj = new S2CQuestPlayEndRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
