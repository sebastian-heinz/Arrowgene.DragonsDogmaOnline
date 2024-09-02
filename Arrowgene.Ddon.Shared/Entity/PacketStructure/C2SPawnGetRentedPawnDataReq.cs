using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetRentedPawnDataReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_RENTED_PAWN_DATA_REQ;

        public C2SPawnGetRentedPawnDataReq()
        {
        }

        public byte SlotNo {  get; set; }
        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnGetRentedPawnDataReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetRentedPawnDataReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnGetRentedPawnDataReq Read(IBuffer buffer)
            {
                C2SPawnGetRentedPawnDataReq obj = new C2SPawnGetRentedPawnDataReq();
                obj.SlotNo = ReadByte(buffer);
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
