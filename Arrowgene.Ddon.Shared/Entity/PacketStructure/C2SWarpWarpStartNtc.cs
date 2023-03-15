using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpWarpStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_WARP_START_NTC;

        public class Serializer : PacketEntitySerializer<C2SWarpWarpStartNtc>
        {
            public override void Write(IBuffer buffer, C2SWarpWarpStartNtc obj)
            {
            }

            public override C2SWarpWarpStartNtc Read(IBuffer buffer)
            {
                C2SWarpWarpStartNtc obj = new C2SWarpWarpStartNtc();
                return obj;
            }
        }

    }
}