using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteCancelNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_CANCEL_NTC;


        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteCancelNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteCancelNtc obj)
            {
            }

            public override S2CPartyPartyInviteCancelNtc Read(IBuffer buffer)
            {
                S2CPartyPartyInviteCancelNtc obj = new S2CPartyPartyInviteCancelNtc();
                return obj;
            }
        }
    }
}
