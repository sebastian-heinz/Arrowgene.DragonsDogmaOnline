using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetAreaBaseInfoListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_AREA_BASE_INFO_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SAreaGetAreaBaseInfoListReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetAreaBaseInfoListReq obj)
            {
            }

            public override C2SAreaGetAreaBaseInfoListReq Read(IBuffer buffer)
            {
                C2SAreaGetAreaBaseInfoListReq obj = new C2SAreaGetAreaBaseInfoListReq();
                return obj;
            }
        }

    }
}