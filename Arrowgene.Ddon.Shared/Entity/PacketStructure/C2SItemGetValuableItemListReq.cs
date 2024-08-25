using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetValuableItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_VALUABLE_ITEM_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SItemGetValuableItemListReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetValuableItemListReq obj)
            {
            }

            public override C2SItemGetValuableItemListReq Read(IBuffer buffer)
            {
                C2SItemGetValuableItemListReq obj = new C2SItemGetValuableItemListReq();
                return obj;
            }
        }
    }
}
