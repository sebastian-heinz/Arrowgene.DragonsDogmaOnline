using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CLogoutRes : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_LOGOUT_RES;

        public class Serializer : PacketEntitySerializer<L2CLogoutRes>
        {
            public override void Write(IBuffer buffer, L2CLogoutRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override L2CLogoutRes Read(IBuffer buffer)
            {
                L2CLogoutRes obj = new L2CLogoutRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
