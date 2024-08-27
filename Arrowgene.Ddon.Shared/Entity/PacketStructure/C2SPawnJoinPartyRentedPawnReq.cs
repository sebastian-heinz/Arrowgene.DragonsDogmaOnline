using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnJoinPartyRentedPawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_JOIN_PARTY_RENTED_PAWN_REQ;

        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnJoinPartyRentedPawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnJoinPartyRentedPawnReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
            }

            public override C2SPawnJoinPartyRentedPawnReq Read(IBuffer buffer)
            {
                C2SPawnJoinPartyRentedPawnReq obj = new C2SPawnJoinPartyRentedPawnReq();
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
