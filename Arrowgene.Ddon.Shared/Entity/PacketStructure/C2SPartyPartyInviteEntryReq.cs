using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyInviteEntryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_INVITE_ENTRY_REQ;

        public class Serializer : PacketEntitySerializer<C2SPartyPartyInviteEntryReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyInviteEntryReq obj)
            {
            }

            public override C2SPartyPartyInviteEntryReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyInviteEntryReq();
            }
        }
    }
}