using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteRefuseRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_REFUSE_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteRefuseRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteRefuseRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyInviteRefuseRes Read(IBuffer buffer)
            {
                S2CPartyPartyInviteRefuseRes obj = new S2CPartyPartyInviteRefuseRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
