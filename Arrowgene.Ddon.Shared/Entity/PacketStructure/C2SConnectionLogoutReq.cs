using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionLogoutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_LOGOUT_REQ;

        public class Serializer : PacketEntitySerializer<C2SConnectionLogoutReq>
        {
            public override void Write(IBuffer buffer, C2SConnectionLogoutReq obj)
            {
            }

            public override C2SConnectionLogoutReq Read(IBuffer buffer)
            {
                C2SConnectionLogoutReq obj = new C2SConnectionLogoutReq();
                return obj;
            }
        }
    }
}
