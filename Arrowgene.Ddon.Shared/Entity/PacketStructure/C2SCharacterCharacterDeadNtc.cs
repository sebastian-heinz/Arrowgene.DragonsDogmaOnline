using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCharacterDeadNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_DEAD_NTC;

        public C2SCharacterCharacterDeadNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterCharacterDeadNtc>
        {
            public override void Write(IBuffer buffer, C2SCharacterCharacterDeadNtc obj)
            {
            }

            public override C2SCharacterCharacterDeadNtc Read(IBuffer buffer)
            {
                C2SCharacterCharacterDeadNtc obj = new C2SCharacterCharacterDeadNtc();
                return obj;
            }
        }
    }
}
