using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestQuestLogInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_QUEST_LOG_INFO_REQ;

        public C2SQuestQuestLogInfoReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SQuestQuestLogInfoReq>
        {
            public override void Write(IBuffer buffer, C2SQuestQuestLogInfoReq obj)
            {
            }

            public override C2SQuestQuestLogInfoReq Read(IBuffer buffer)
            {
                C2SQuestQuestLogInfoReq obj = new C2SQuestQuestLogInfoReq();
                return obj;
            }
        }
    }
}
