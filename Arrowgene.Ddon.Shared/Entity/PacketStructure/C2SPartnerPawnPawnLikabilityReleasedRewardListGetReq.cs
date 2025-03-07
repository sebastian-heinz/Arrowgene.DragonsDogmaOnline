using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_REQ;

        public C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq>
        {
            public override void Write(IBuffer buffer, C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq obj)
            {
            }

            public override C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq Read(IBuffer buffer)
            {
                C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq obj = new C2SPartnerPawnPawnLikabilityReleasedRewardListGetReq();

                return obj;
            }
        }
    }
}
