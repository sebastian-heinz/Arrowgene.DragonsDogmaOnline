using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    // The client does literally nothing when receiving this.
    public class S2CPartyPartyInviteSuccessNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_SUCCESS_NTC;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteSuccessNtc> {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteSuccessNtc obj)
            {
            }

            public override S2CPartyPartyInviteSuccessNtc Read(IBuffer buffer)
            {
                return new S2CPartyPartyInviteSuccessNtc();
            }
        }
    }
}