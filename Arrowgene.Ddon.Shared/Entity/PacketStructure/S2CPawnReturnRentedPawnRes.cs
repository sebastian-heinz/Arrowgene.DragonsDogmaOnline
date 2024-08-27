using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnReturnRentedPawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_RETURN_RENTED_PAWN_RES;

        public S2CPawnReturnRentedPawnRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CPawnReturnRentedPawnRes>
        {
            public override void Write(IBuffer buffer, S2CPawnReturnRentedPawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnReturnRentedPawnRes Read(IBuffer buffer)
            {
                S2CPawnReturnRentedPawnRes obj = new S2CPawnReturnRentedPawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}

