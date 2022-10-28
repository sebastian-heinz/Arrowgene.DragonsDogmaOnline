using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyBreakupRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_BREAKUP_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyBreakupRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyBreakupRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyBreakupRes Read(IBuffer buffer)
            {
                S2CPartyPartyBreakupRes obj = new S2CPartyPartyBreakupRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
