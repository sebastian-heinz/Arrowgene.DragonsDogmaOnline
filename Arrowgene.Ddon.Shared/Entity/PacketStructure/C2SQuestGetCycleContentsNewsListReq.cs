using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetCycleContentsNewsListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_CYCLE_CONTENTS_NEWS_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetCycleContentsNewsListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetCycleContentsNewsListReq obj)
            {
            }

            public override C2SQuestGetCycleContentsNewsListReq Read(IBuffer buffer)
            {
                C2SQuestGetCycleContentsNewsListReq obj = new C2SQuestGetCycleContentsNewsListReq();
                return obj;
            }
        }
    }
}
