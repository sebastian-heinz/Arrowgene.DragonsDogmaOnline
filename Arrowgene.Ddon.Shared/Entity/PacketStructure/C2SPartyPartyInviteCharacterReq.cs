using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyInviteCharacterReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_INVITE_CHARACTER_REQ;

        public C2SPartyPartyInviteCharacterReq()
        {
            CharacterId = 0;
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartyPartyInviteCharacterReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyInviteCharacterReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SPartyPartyInviteCharacterReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyInviteCharacterReq
                {
                    CharacterId = ReadUInt32(buffer)
                };
            }
        }
    }
}