using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetQuestPartyBonusListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_QUEST_PARTY_BONUS_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetQuestPartyBonusListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetQuestPartyBonusListReq obj)
            {
            }

            public override C2SQuestGetQuestPartyBonusListReq Read(IBuffer buffer)
            {
                C2SQuestGetQuestPartyBonusListReq obj = new C2SQuestGetQuestPartyBonusListReq();
                return obj;
            }
        }
    }
}
