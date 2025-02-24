using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnPawnLikabilityRewardListGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_RES;

        public S2CPartnerPawnPawnLikabilityRewardListGetRes()
        {
            RewardList = new List<CDataPartnerPawnReward>();
        }

        public List<CDataPartnerPawnReward> RewardList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnPawnLikabilityRewardListGetRes>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnPawnLikabilityRewardListGetRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RewardList);
            }

            public override S2CPartnerPawnPawnLikabilityRewardListGetRes Read(IBuffer buffer)
            {
                S2CPartnerPawnPawnLikabilityRewardListGetRes obj = new S2CPartnerPawnPawnLikabilityRewardListGetRes();
                ReadServerResponse(buffer, obj);
                obj.RewardList = ReadEntityList<CDataPartnerPawnReward>(buffer);
                return obj;
            }
        }
    }
}
