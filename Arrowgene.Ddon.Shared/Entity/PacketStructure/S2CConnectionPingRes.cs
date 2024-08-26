using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionPingRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_PING_RES;

        public S2CConnectionPingRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CConnectionPingRes>
        {
            public override void Write(IBuffer buffer, S2CConnectionPingRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CConnectionPingRes Read(IBuffer buffer)
            {
                S2CConnectionPingRes obj = new S2CConnectionPingRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}