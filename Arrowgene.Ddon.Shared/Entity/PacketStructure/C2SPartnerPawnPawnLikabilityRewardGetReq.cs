using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartnerPawnPawnLikabilityRewardGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_REQ;

        public List<CDataPartnerPawnReward> RewardUidList { get; set; } = new();
        /// <summary>
        /// TODO: What is this for? Probably populated by the client.
        /// </summary>
        public ulong UpdateHairUid { get; set; }

        public C2SPartnerPawnPawnLikabilityRewardGetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPartnerPawnPawnLikabilityRewardGetReq>
        {
            public override void Write(IBuffer buffer, C2SPartnerPawnPawnLikabilityRewardGetReq obj)
            {
                WriteEntityList<CDataPartnerPawnReward>(buffer, obj.RewardUidList);
                WriteUInt64(buffer, obj.UpdateHairUid);
            }

            public override C2SPartnerPawnPawnLikabilityRewardGetReq Read(IBuffer buffer)
            {
                C2SPartnerPawnPawnLikabilityRewardGetReq obj = new C2SPartnerPawnPawnLikabilityRewardGetReq();

                obj.RewardUidList = ReadEntityList<CDataPartnerPawnReward>(buffer);
                obj.UpdateHairUid = ReadUInt64(buffer);

                return obj;
            }
        }
    }
}
