using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanInviteRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_INVITE_RES;

        public class Serializer : PacketEntitySerializer<S2CClanClanInviteRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanInviteRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanInviteRes Read(IBuffer buffer)
            {
                S2CClanClanInviteRes obj = new S2CClanClanInviteRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
