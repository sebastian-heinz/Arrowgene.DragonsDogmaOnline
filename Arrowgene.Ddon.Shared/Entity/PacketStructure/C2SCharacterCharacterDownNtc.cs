using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCharacterDownNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_DOWN_NTC;

        public C2SCharacterCharacterDownNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterCharacterDownNtc>
        {
            public override void Write(IBuffer buffer, C2SCharacterCharacterDownNtc obj)
            {
            }

            public override C2SCharacterCharacterDownNtc Read(IBuffer buffer)
            {
                C2SCharacterCharacterDownNtc obj = new C2SCharacterCharacterDownNtc();
                return obj;
            }
        }
    }
}
