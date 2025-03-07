using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartnerPawnPawnLikabilityRewardListGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_REQ;


        public C2SPartnerPawnPawnLikabilityRewardListGetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPartnerPawnPawnLikabilityRewardListGetReq>
        {
            public override void Write(IBuffer buffer, C2SPartnerPawnPawnLikabilityRewardListGetReq obj)
            {
            }

            public override C2SPartnerPawnPawnLikabilityRewardListGetReq Read(IBuffer buffer)
            {
                C2SPartnerPawnPawnLikabilityRewardListGetReq obj = new C2SPartnerPawnPawnLikabilityRewardListGetReq();
                return obj;
            }
        }
    }
}
