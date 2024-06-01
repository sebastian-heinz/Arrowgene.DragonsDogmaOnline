using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetRewardBoxListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_REWARD_BOX_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetRewardBoxListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetRewardBoxListReq obj)
            {
            }

            public override C2SQuestGetRewardBoxListReq Read(IBuffer buffer)
            {
                return new C2SQuestGetRewardBoxListReq();
            }
        }
    }
}
