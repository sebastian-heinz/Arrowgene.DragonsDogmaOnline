using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnPawnLostNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_PAWN_LOST_NTC;

        public S2CPawnPawnLostNtc()
        {
            PawnName = string.Empty;
        }

        public uint PawnId { get; set; }
        public string PawnName { get; set; }
        public bool IsLost { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnPawnLostNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnPawnLostNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteMtString(buffer, obj.PawnName);
                WriteBool(buffer, obj.IsLost);
            }

            public override S2CPawnPawnLostNtc Read(IBuffer buffer)
            {
                S2CPawnPawnLostNtc obj = new S2CPawnPawnLostNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnName = ReadMtString(buffer);
                obj.IsLost = ReadBool(buffer);
                return obj;
            }
        }

    }
}