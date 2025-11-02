using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SOrbDevoteGetReleaseOrbElementListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SOrbDevoteGetReleaseOrbElementListReq>
        {
            public override void Write(IBuffer buffer, C2SOrbDevoteGetReleaseOrbElementListReq obj)
            {
            }

            public override C2SOrbDevoteGetReleaseOrbElementListReq Read(IBuffer buffer)
            {
                C2SOrbDevoteGetReleaseOrbElementListReq obj = new C2SOrbDevoteGetReleaseOrbElementListReq();
                return obj;
            }
        }
    }
}
