using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGetFreeRentalPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_FREE_RENTAL_PAWN_LIST_REQ;

        public C2SGetFreeRentalPawnListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGetFreeRentalPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SGetFreeRentalPawnListReq obj)
            {
            }

            public override C2SGetFreeRentalPawnListReq Read(IBuffer buffer)
            {
                C2SGetFreeRentalPawnListReq obj = new C2SGetFreeRentalPawnListReq();

                return obj;
            }
        }
    }
}
