using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetAreaReleaseListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_AREA_RELEASE_LIST_REQ;


        public class Serializer : PacketEntitySerializer<C2SAreaGetAreaReleaseListReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetAreaReleaseListReq obj)
            {
            }

            public override C2SAreaGetAreaReleaseListReq Read(IBuffer buffer)
            {
                C2SAreaGetAreaReleaseListReq obj = new C2SAreaGetAreaReleaseListReq();
                return obj;
            }
        }
    }
}
