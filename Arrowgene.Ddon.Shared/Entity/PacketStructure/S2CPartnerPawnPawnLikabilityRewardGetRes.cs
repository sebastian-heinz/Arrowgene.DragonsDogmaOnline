using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnPawnLikabilityRewardGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_RES;

        public List<CDataPartnerPawnReward> RewardList { get; set; }

        public S2CPartnerPawnPawnLikabilityRewardGetRes()
        {
            RewardList = new List<CDataPartnerPawnReward>();
        }

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnPawnLikabilityRewardGetRes>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnPawnLikabilityRewardGetRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataPartnerPawnReward>(buffer, obj.RewardList);
            }

            public override S2CPartnerPawnPawnLikabilityRewardGetRes Read(IBuffer buffer)
            {
                S2CPartnerPawnPawnLikabilityRewardGetRes obj = new S2CPartnerPawnPawnLikabilityRewardGetRes();

                ReadServerResponse(buffer, obj);

                obj.RewardList = ReadEntityList<CDataPartnerPawnReward>(buffer);

                return obj;
            }
        }
    }
}
