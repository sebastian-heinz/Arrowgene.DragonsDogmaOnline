using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInnGetPenaltyHealStayPriceReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INN_GET_PENALTY_HEAL_STAY_PRICE_REQ;

        public C2SInnGetPenaltyHealStayPriceReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SInnGetPenaltyHealStayPriceReq>
        {
            public override void Write(IBuffer buffer, C2SInnGetPenaltyHealStayPriceReq obj)
            {
            }

            public override C2SInnGetPenaltyHealStayPriceReq Read(IBuffer buffer)
            {
                C2SInnGetPenaltyHealStayPriceReq obj = new C2SInnGetPenaltyHealStayPriceReq();
                return obj;
            }
        }

    }
}