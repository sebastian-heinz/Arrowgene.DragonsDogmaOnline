using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteEntryRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_ENTRY_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteEntryRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteEntryRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyInviteEntryRes Read(IBuffer buffer)
            {
                S2CPartyPartyInviteEntryRes obj = new S2CPartyPartyInviteEntryRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}