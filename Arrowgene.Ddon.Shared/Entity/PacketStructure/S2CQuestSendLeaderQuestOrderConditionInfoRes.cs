using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSendLeaderQuestOrderConditionInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_RES;

        public class Serializer : PacketEntitySerializer<S2CQuestSendLeaderQuestOrderConditionInfoRes>
        {
            public override void Write(IBuffer buffer, S2CQuestSendLeaderQuestOrderConditionInfoRes packet)
            {
                WriteServerResponse(buffer, packet);
            }

            public override S2CQuestSendLeaderQuestOrderConditionInfoRes Read(IBuffer buffer)
            {
                S2CQuestSendLeaderQuestOrderConditionInfoRes obj = new S2CQuestSendLeaderQuestOrderConditionInfoRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}