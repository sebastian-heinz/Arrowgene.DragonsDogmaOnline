using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetLeaderAreaReleaseListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_LEADER_AREA_RELEASE_LIST_REQ;

        public C2SAreaGetLeaderAreaReleaseListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SAreaGetLeaderAreaReleaseListReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetLeaderAreaReleaseListReq obj)
            {
            }

            public override C2SAreaGetLeaderAreaReleaseListReq Read(IBuffer buffer)
            {
                C2SAreaGetLeaderAreaReleaseListReq obj = new C2SAreaGetLeaderAreaReleaseListReq();
                return obj;
            }
        }
    }
}

