using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyInvitePrepareAcceptReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_INVITE_PREPARE_ACCEPT_REQ;

        public class Serializer : PacketEntitySerializer<C2SPartyPartyInvitePrepareAcceptReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyInvitePrepareAcceptReq obj)
            {
            }

            public override C2SPartyPartyInvitePrepareAcceptReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyInvitePrepareAcceptReq();
            }
        }
    }
}