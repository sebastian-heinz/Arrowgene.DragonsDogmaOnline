using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetFreeRentalPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_FREE_RENTAL_PAWN_LIST_REQ;

        public C2SPawnGetFreeRentalPawnListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPawnGetFreeRentalPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetFreeRentalPawnListReq obj)
            {
            }

            public override C2SPawnGetFreeRentalPawnListReq Read(IBuffer buffer)
            {
                C2SPawnGetFreeRentalPawnListReq obj = new C2SPawnGetFreeRentalPawnListReq();

                return obj;
            }
        }
    }
}
