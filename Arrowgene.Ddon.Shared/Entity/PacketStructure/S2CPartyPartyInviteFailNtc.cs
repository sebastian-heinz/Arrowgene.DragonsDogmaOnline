using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteFailNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_FAIL_NTC;

        public ErrorCode ErrorCode { get; set; }
        public ushort ServerId { get; set; }
        public uint PartyId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteFailNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteFailNtc obj)
            {
                WriteUInt32(buffer, (uint)obj.ErrorCode);
                WriteUInt16(buffer, obj.ServerId);
                WriteUInt32(buffer, obj.PartyId);
            }

            public override S2CPartyPartyInviteFailNtc Read(IBuffer buffer)
            {
                return new S2CPartyPartyInviteFailNtc
                {
                    ErrorCode = (ErrorCode)ReadUInt32(buffer),
                    ServerId = ReadUInt16(buffer),
                    PartyId = ReadUInt32(buffer)
                };
            }
        }
    }
}
