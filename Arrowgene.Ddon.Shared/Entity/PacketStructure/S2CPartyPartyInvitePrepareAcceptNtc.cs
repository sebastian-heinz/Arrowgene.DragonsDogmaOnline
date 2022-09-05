using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    // This packet is for when a player requests to join a party, and the leader accepts?
    public class S2CPartyPartyInvitePrepareAcceptNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_PREPARE_ACCEPT_NTC;
        public class Serializer : PacketEntitySerializer<S2CPartyPartyInvitePrepareAcceptNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInvitePrepareAcceptNtc obj)
            {
            }

            public override S2CPartyPartyInvitePrepareAcceptNtc Read(IBuffer buffer)
            {
                return new S2CPartyPartyInvitePrepareAcceptNtc();
            }
        }
    }
}