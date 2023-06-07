using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpPartyWarpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_PARTY_WARP_REQ;

        public class Serializer : PacketEntitySerializer<C2SWarpPartyWarpReq>
        {
            public override void Write(IBuffer buffer, C2SWarpPartyWarpReq obj)
            {
            }

            public override C2SWarpPartyWarpReq Read(IBuffer buffer)
            {
                C2SWarpPartyWarpReq obj = new C2SWarpPartyWarpReq();
                return obj;
            }
        }

    }
}