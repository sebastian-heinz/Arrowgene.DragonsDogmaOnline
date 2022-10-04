using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyMemberKickNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_MEMBER_KICK_NTC;

        public byte MemberIndex { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyMemberKickNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyMemberKickNtc obj)
            {
                WriteByte(buffer, obj.MemberIndex);
            }

            public override S2CPartyPartyMemberKickNtc Read(IBuffer buffer)
            {
                S2CPartyPartyMemberKickNtc obj = new S2CPartyPartyMemberKickNtc();
                obj.MemberIndex = ReadByte(buffer);
                return obj;
            }
        }
    }
}
