using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyLeaveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_LEAVE_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyLeaveRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyLeaveRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyLeaveRes Read(IBuffer buffer)
            {
                S2CPartyPartyLeaveRes obj = new S2CPartyPartyLeaveRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}