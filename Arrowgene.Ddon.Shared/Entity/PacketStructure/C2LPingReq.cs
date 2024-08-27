using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LPingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_PING_REQ;

        public class Serializer : PacketEntitySerializer<C2LPingReq>
        {
            public override void Write(IBuffer buffer, C2LPingReq obj)
            {
            }

            public override C2LPingReq Read(IBuffer buffer)
            {
                C2LPingReq obj = new C2LPingReq();
                return obj;
            }
        }

    }
}