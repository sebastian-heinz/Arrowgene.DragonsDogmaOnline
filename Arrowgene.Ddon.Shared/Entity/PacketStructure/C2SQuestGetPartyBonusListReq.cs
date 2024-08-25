using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetPartyBonusListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_PARTY_BONUS_LIST_REQ;

        public C2SQuestGetPartyBonusListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SQuestGetPartyBonusListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetPartyBonusListReq obj)
            {
            }

            public override C2SQuestGetPartyBonusListReq Read(IBuffer buffer)
            {
                C2SQuestGetPartyBonusListReq obj = new C2SQuestGetPartyBonusListReq();
                return obj;
            }
        }
    }
}
