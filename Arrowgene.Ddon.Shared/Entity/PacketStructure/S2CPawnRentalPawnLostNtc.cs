using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnRentalPawnLostNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_RENTAL_PAWN_LOST_NTC;

        public S2CPawnRentalPawnLostNtc()
        {
            PawnName = string.Empty;
        }

        public uint PawnId { get; set; }
        public string PawnName { get; set; }
        public byte AdventureCount { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnRentalPawnLostNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnRentalPawnLostNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteMtString(buffer, obj.PawnName);
                WriteByte(buffer, obj.AdventureCount);
            }

            public override S2CPawnRentalPawnLostNtc Read(IBuffer buffer)
            {
                S2CPawnRentalPawnLostNtc obj = new S2CPawnRentalPawnLostNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnName = ReadMtString(buffer);
                obj.AdventureCount = ReadByte(buffer);
                return obj;
            }
        }

    }
}