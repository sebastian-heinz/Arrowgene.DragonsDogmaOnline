using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterPawnDownNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_PAWN_DOWN_NTC;

        public C2SCharacterPawnDownNtc()
        {
        }

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterPawnDownNtc>
        {
            public override void Write(IBuffer buffer, C2SCharacterPawnDownNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SCharacterPawnDownNtc Read(IBuffer buffer)
            {
                C2SCharacterPawnDownNtc obj = new C2SCharacterPawnDownNtc();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
