using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanInviteAcceptRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_INVITE_ACCEPT_RES;

        public class Serializer : PacketEntitySerializer<S2CClanClanInviteAcceptRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanInviteAcceptRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanInviteAcceptRes Read(IBuffer buffer)
            {
                S2CClanClanInviteAcceptRes obj = new S2CClanClanInviteAcceptRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
