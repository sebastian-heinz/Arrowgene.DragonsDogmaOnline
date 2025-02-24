using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_RES;

        public List<CDataPartnerPawnReward> ReleasedRewardList { get; set; } = new();

        public S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataPartnerPawnReward>(buffer, obj.ReleasedRewardList);
            }

            public override S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes Read(IBuffer buffer)
            {
                S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes obj = new S2CPartnerPawnPawnLikabilityReleasedRewardListGetRes();

                ReadServerResponse(buffer, obj);

                obj.ReleasedRewardList = ReadEntityList<CDataPartnerPawnReward>(buffer);

                return obj;
            }
        }
    }
}
