using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionPingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_PING_REQ;

        public C2SConnectionPingReq()
        {   
        }

        public class Serializer : PacketEntitySerializer<C2SConnectionPingReq>
        {
            public override void Write(IBuffer buffer, C2SConnectionPingReq obj)
            {
            }

            public override C2SConnectionPingReq Read(IBuffer buffer)
            {
                C2SConnectionPingReq obj = new C2SConnectionPingReq();
                return obj;
            }
        }

    }
}