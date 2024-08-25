using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetPostItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_POST_ITEM_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SItemGetPostItemListReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetPostItemListReq obj)
            {
            }

            public override C2SItemGetPostItemListReq Read(IBuffer buffer)
            {
                C2SItemGetPostItemListReq obj = new C2SItemGetPostItemListReq();
                return obj;
            }
        }
    }
}
