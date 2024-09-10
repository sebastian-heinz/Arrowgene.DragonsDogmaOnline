using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnRentRegisteredPawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_RENT_REGISTERED_PAWN_RES;

        public S2CPawnRentRegisteredPawnRes()
        {
        }

        public uint TotalRim { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnRentRegisteredPawnRes>
        {
            public override void Write(IBuffer buffer, S2CPawnRentRegisteredPawnRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.TotalRim);
            }

            public override S2CPawnRentRegisteredPawnRes Read(IBuffer buffer)
            {
                S2CPawnRentRegisteredPawnRes obj = new S2CPawnRentRegisteredPawnRes();
                ReadServerResponse(buffer, obj);
                obj.TotalRim = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
