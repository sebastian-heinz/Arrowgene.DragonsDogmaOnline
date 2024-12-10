using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnRentalPawnLostRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_RENTAL_PAWN_LOST_RES;

        public S2CPawnRentalPawnLostRes()
        {
            PawnName = string.Empty;
        }

        public uint PawnId { get; set; }
        public string PawnName { get; set; }
        public byte AdventureCount { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnRentalPawnLostRes>
        {
            public override void Write(IBuffer buffer, S2CPawnRentalPawnLostRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteMtString(buffer, obj.PawnName);
                WriteByte(buffer, obj.AdventureCount);
            }

            public override S2CPawnRentalPawnLostRes Read(IBuffer buffer)
            {
                S2CPawnRentalPawnLostRes obj = new S2CPawnRentalPawnLostRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnName = ReadMtString(buffer);
                obj.AdventureCount = ReadByte(buffer);
                return obj;
            }
        }
    }
}