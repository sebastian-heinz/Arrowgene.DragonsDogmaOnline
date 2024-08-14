using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCharacterDownCancelNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_DOWN_CANCEL_NTC;

        public C2SCharacterCharacterDownCancelNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterCharacterDownCancelNtc>
        {
            public override void Write(IBuffer buffer, C2SCharacterCharacterDownCancelNtc obj)
            {
            }

            public override C2SCharacterCharacterDownCancelNtc Read(IBuffer buffer)
            {
                C2SCharacterCharacterDownCancelNtc obj = new C2SCharacterCharacterDownCancelNtc();
                return obj;
            }
        }
    }
}
