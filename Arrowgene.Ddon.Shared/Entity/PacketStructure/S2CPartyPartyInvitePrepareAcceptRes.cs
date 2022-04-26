using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInvitePrepareAcceptRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_PREPARE_ACCEPT_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInvitePrepareAcceptRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInvitePrepareAcceptRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyInvitePrepareAcceptRes Read(IBuffer buffer)
            {
                S2CPartyPartyInvitePrepareAcceptRes obj = new S2CPartyPartyInvitePrepareAcceptRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}