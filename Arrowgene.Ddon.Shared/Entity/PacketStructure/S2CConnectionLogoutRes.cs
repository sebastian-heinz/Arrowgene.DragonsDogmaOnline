using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionLogoutRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_LOGOUT_RES;

        public class Serializer : PacketEntitySerializer<S2CConnectionLogoutRes>
        {

            public override void Write(IBuffer buffer, S2CConnectionLogoutRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CConnectionLogoutRes Read(IBuffer buffer)
            {
                S2CConnectionLogoutRes obj = new S2CConnectionLogoutRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
