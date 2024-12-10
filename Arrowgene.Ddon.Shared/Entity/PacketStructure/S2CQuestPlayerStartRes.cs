using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayerStartRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_PLAY_START_RES;

        public S2CQuestPlayerStartRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayerStartRes>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayerStartRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestPlayerStartRes Read(IBuffer buffer)
            {
                S2CQuestPlayerStartRes obj = new S2CQuestPlayerStartRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
