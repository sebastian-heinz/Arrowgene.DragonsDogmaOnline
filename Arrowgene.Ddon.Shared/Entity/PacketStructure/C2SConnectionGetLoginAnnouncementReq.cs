using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionGetLoginAnnouncementReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_GET_LOGIN_ANNOUNCEMENT_REQ;
        public class Serializer : PacketEntitySerializer<C2SConnectionGetLoginAnnouncementReq>
        {
            public override void Write(IBuffer buffer, C2SConnectionGetLoginAnnouncementReq obj)
            {
            }

            public override C2SConnectionGetLoginAnnouncementReq Read(IBuffer buffer)
            {
                C2SConnectionGetLoginAnnouncementReq obj = new C2SConnectionGetLoginAnnouncementReq();
                return obj;
            }
        }
    }
}
