using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSendLeaderWaitOrderQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_RES;

        public class Serializer : PacketEntitySerializer<S2CQuestSendLeaderWaitOrderQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestSendLeaderWaitOrderQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CQuestSendLeaderWaitOrderQuestListRes Read(IBuffer buffer)
            {
                S2CQuestSendLeaderWaitOrderQuestListRes obj = new S2CQuestSendLeaderWaitOrderQuestListRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}