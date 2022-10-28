using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyMemberKickReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_MEMBER_KICK_REQ;

        public byte MemberIndex { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartyPartyMemberKickReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyMemberKickReq obj)
            {
                WriteByte(buffer, obj.MemberIndex);
            }

            public override C2SPartyPartyMemberKickReq Read(IBuffer buffer)
            {
                C2SPartyPartyMemberKickReq obj = new C2SPartyPartyMemberKickReq();
                obj.MemberIndex = ReadByte(buffer);
                return obj;
            }
        }
    }
}
