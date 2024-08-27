using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnJoinPartyRentedPawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_JOIN_PARTY_RENTED_PAWN_RES;

        public S2CPawnJoinPartyRentedPawnRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CPawnJoinPartyRentedPawnRes>
        {
            public override void Write(IBuffer buffer, S2CPawnJoinPartyRentedPawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnJoinPartyRentedPawnRes Read(IBuffer buffer)
            {
                S2CPawnJoinPartyRentedPawnRes obj = new S2CPawnJoinPartyRentedPawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
