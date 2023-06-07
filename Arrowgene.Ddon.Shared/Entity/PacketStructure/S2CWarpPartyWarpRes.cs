using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpPartyWarpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_PARTY_WARP_RES;

        public class Serializer : PacketEntitySerializer<S2CWarpPartyWarpRes>
        {
            public override void Write(IBuffer buffer, S2CWarpPartyWarpRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CWarpPartyWarpRes Read(IBuffer buffer)
            {
                S2CWarpPartyWarpRes obj = new S2CWarpPartyWarpRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}