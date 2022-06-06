using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnPawnLikabilityRewardListGetRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_RES;

        public S2CPartnerPawnPawnLikabilityRewardListGetRes()
        {
        }

        public C2SPartnerPawnPawnLikabilityRewardListGetReq ReqData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnPawnLikabilityRewardListGetRes>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnPawnLikabilityRewardListGetRes obj)
            {
                WriteByteArray(buffer, Data);
            }

            public override S2CPartnerPawnPawnLikabilityRewardListGetRes Read(IBuffer buffer)
            {
                S2CPartnerPawnPawnLikabilityRewardListGetRes obj = new S2CPartnerPawnPawnLikabilityRewardListGetRes();
                return obj;
            }


            private readonly byte[] Data =
            {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x80, 0x0, 0x0, 0x0,
                0x7, 0x43, 0xB, 0xCA, 0x40, 0x1, 0x0
            };
        }

    }
}
