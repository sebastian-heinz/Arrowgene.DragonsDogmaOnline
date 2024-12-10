using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetPriorityQuestReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_PRIORITY_QUEST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetPriorityQuestReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetPriorityQuestReq obj)
            {
            }

            public override C2SQuestGetPriorityQuestReq Read(IBuffer buffer)
            {
                C2SQuestGetPriorityQuestReq obj = new C2SQuestGetPriorityQuestReq();
                return obj;
            }
        }
    }
}
