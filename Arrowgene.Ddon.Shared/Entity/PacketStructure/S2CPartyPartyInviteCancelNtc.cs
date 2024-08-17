using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteCancelNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_CANCEL_NTC;

        public ErrorCode ErrorCode { get; set; }

        public S2CPartyPartyInviteCancelNtc()
        {
            ErrorCode = 0;
        }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteCancelNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteCancelNtc obj)
            {
                WriteUInt32(buffer, (uint)obj.ErrorCode);
            }

            public override S2CPartyPartyInviteCancelNtc Read(IBuffer buffer)
            {
                S2CPartyPartyInviteCancelNtc obj = new S2CPartyPartyInviteCancelNtc();
                obj.ErrorCode = (ErrorCode)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
