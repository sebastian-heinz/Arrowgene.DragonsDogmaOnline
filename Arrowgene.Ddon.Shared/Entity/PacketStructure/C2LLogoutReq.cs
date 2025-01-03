using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LLogoutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_LOGOUT_REQ;

        public class Serializer : PacketEntitySerializer<C2LLogoutReq>
        {

            public override void Write(IBuffer buffer, C2LLogoutReq obj)
            {
            }

            public override C2LLogoutReq Read(IBuffer buffer)
            {
                C2LLogoutReq obj = new C2LLogoutReq();
                return obj;
            }
        }
    }
}
