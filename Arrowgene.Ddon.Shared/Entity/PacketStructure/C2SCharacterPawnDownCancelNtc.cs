using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterPawnDownCancelNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_PAWN_DOWN_CANCEL_NTC;

        public C2SCharacterPawnDownCancelNtc()
        {
        }

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterPawnDownCancelNtc>
        {
            public override void Write(IBuffer buffer, C2SCharacterPawnDownCancelNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SCharacterPawnDownCancelNtc Read(IBuffer buffer)
            {
                C2SCharacterPawnDownCancelNtc obj = new C2SCharacterPawnDownCancelNtc();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
