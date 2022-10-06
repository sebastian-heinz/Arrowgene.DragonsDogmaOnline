using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteCancelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_CANCEL_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteCancelRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteCancelRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyInviteCancelRes Read(IBuffer buffer)
            {
                S2CPartyPartyInviteCancelRes obj = new S2CPartyPartyInviteCancelRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
