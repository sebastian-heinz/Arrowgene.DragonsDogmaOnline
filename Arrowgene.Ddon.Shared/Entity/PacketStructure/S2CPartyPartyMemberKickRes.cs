using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyMemberKickRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_MEMBER_KICK_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyMemberKickRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyMemberKickRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyMemberKickRes Read(IBuffer buffer)
            {
                S2CPartyPartyMemberKickRes obj = new S2CPartyPartyMemberKickRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
