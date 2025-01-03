using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestEndDistributionQuestCancelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_END_DISTRIBUTION_QUEST_CANCEL_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestEndDistributionQuestCancelReq>
        {
            public override void Write(IBuffer buffer, C2SQuestEndDistributionQuestCancelReq obj)
            {
            }

            public override C2SQuestEndDistributionQuestCancelReq Read(IBuffer buffer)
            {
                C2SQuestEndDistributionQuestCancelReq obj = new C2SQuestEndDistributionQuestCancelReq();
                return obj;
            }
        }
    }
}
