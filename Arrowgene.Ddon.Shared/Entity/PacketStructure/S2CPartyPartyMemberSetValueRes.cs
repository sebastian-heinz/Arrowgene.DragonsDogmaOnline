using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyMemberSetValueRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_MEMBER_SET_VALUE_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyMemberSetValueRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyMemberSetValueRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyMemberSetValueRes Read(IBuffer buffer)
            {
                S2CPartyPartyMemberSetValueRes obj = new S2CPartyPartyMemberSetValueRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
