using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyInviteRefuseReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_INVITE_REFUSE_REQ;

        public class Serializer : PacketEntitySerializer<C2SPartyPartyInviteRefuseReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyInviteRefuseReq obj)
            {
            }

            public override C2SPartyPartyInviteRefuseReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyInviteRefuseReq();
            }
        }
    }
}
