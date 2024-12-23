using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetPartyQuestProgressInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_REQ;

        public C2SQuestGetPartyQuestProgressInfoReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SQuestGetPartyQuestProgressInfoReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetPartyQuestProgressInfoReq obj)
            {
            }

            public override C2SQuestGetPartyQuestProgressInfoReq Read(IBuffer buffer)
            {
                C2SQuestGetPartyQuestProgressInfoReq obj = new C2SQuestGetPartyQuestProgressInfoReq();
                return obj;
            }
        }
    }
}
