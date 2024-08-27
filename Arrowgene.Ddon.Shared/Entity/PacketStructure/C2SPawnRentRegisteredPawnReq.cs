using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnRentRegisteredPawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_RENT_REGISTERED_PAWN_REQ;

        public byte SlotNo { get; set; }
        public uint RequestedPawnId { get; set; }
        public ulong Updated { get; set; }
        public uint RentalCost { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnRentRegisteredPawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnRentRegisteredPawnReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.RequestedPawnId);
                WriteUInt64(buffer, obj.Updated);
                WriteUInt32(buffer, obj.RentalCost);
            }

            public override C2SPawnRentRegisteredPawnReq Read(IBuffer buffer)
            {
                C2SPawnRentRegisteredPawnReq obj = new C2SPawnRentRegisteredPawnReq();
                obj.SlotNo = ReadByte(buffer);
                obj.RequestedPawnId = ReadUInt32(buffer);
                obj.Updated = ReadUInt64(buffer);
                obj.RentalCost = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
