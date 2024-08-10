using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterDecideCharacterIdReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ;

        public C2SCharacterDecideCharacterIdReq()
        {
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterDecideCharacterIdReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterDecideCharacterIdReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SCharacterDecideCharacterIdReq Read(IBuffer buffer)
            {
                C2SCharacterDecideCharacterIdReq obj = new C2SCharacterDecideCharacterIdReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
