using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnRentalPawnLostReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_RENTAL_PAWN_LOST_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnRentalPawnLostReq>
        {
            public override void Write(IBuffer buffer, C2SPawnRentalPawnLostReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnRentalPawnLostReq Read(IBuffer buffer)
            {
                C2SPawnRentalPawnLostReq obj = new C2SPawnRentalPawnLostReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}