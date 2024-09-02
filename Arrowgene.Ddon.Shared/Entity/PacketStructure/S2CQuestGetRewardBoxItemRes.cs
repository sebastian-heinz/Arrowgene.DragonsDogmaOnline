using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetRewardBoxItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_REWARD_BOX_ITEM_RES;

        public class Serializer : PacketEntitySerializer<S2CQuestGetRewardBoxItemRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetRewardBoxItemRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestGetRewardBoxItemRes Read(IBuffer buffer)
            {
                S2CQuestGetRewardBoxItemRes obj = new S2CQuestGetRewardBoxItemRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
