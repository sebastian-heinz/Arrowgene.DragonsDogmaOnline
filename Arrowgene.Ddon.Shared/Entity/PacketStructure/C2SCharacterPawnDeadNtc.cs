using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterPawnDeadNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_PAWN_DEAD_NTC;

        public C2SCharacterPawnDeadNtc()
        {
        }

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterPawnDeadNtc>
        {
            public override void Write(IBuffer buffer, C2SCharacterPawnDeadNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SCharacterPawnDeadNtc Read(IBuffer buffer)
            {
                C2SCharacterPawnDeadNtc obj = new C2SCharacterPawnDeadNtc();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
