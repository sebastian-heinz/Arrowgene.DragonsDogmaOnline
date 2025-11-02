using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetRentedPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_RENTED_PAWN_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnGetRentedPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetRentedPawnListReq obj)
            {
            }

            public override C2SPawnGetRentedPawnListReq Read(IBuffer buffer)
            {
                C2SPawnGetRentedPawnListReq obj = new C2SPawnGetRentedPawnListReq();
                return obj;
            }
        }
    }
}
