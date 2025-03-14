using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartnerPawnNextPresentTimeGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_REQ;

        public C2SPartnerPawnNextPresentTimeGetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPartnerPawnNextPresentTimeGetReq>
        {
            public override void Write(IBuffer buffer, C2SPartnerPawnNextPresentTimeGetReq obj)
            {
            }

            public override C2SPartnerPawnNextPresentTimeGetReq Read(IBuffer buffer)
            {
                C2SPartnerPawnNextPresentTimeGetReq obj = new C2SPartnerPawnNextPresentTimeGetReq();

                return obj;
            }
        }
    }
}
