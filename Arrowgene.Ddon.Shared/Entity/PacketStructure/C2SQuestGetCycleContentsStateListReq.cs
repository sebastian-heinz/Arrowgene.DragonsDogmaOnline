using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetCycleContentsStateListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_REQ;

        public C2SQuestGetCycleContentsStateListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SQuestGetCycleContentsStateListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetCycleContentsStateListReq obj)
            {
            }

            public override C2SQuestGetCycleContentsStateListReq Read(IBuffer buffer)
            {
                C2SQuestGetCycleContentsStateListReq obj = new C2SQuestGetCycleContentsStateListReq();
                return obj;
            }
        }
    }
}
