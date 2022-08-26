using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyMemberSessionStatusNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_MEMBER_SESSION_STATUS_NTC;

        public uint CharacterId { get; set; }
        public byte SessionStatus { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyMemberSessionStatusNtc> {
            public override void Write(IBuffer buffer, S2CPartyPartyMemberSessionStatusNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.SessionStatus);
            }

            public override S2CPartyPartyMemberSessionStatusNtc Read(IBuffer buffer)
            {
                S2CPartyPartyMemberSessionStatusNtc obj = new S2CPartyPartyMemberSessionStatusNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.SessionStatus = ReadByte(buffer);
                return obj;
            }
        }
    }
}