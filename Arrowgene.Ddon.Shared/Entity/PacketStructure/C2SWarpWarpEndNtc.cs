using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpWarpEndNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_WARP_WARP_END_NTC;

        public class Serializer : PacketEntitySerializer<C2SWarpWarpEndNtc>
        {
            public override void Write(IBuffer buffer, C2SWarpWarpEndNtc obj)
            {   
            }

            public override C2SWarpWarpEndNtc Read(IBuffer buffer)
            {
                C2SWarpWarpEndNtc obj = new C2SWarpWarpEndNtc();
                return obj;
            }
        }

    }
}