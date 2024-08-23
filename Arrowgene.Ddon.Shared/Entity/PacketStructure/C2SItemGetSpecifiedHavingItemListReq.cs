using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetSpecifiedHavingItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_REQ;

        public C2SItemGetSpecifiedHavingItemListReq()
        {
        }

        public uint ItemId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SItemGetSpecifiedHavingItemListReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetSpecifiedHavingItemListReq obj)
            {
                WriteUInt32(buffer, obj.ItemId);
            }

            public override C2SItemGetSpecifiedHavingItemListReq Read(IBuffer buffer)
            {
                C2SItemGetSpecifiedHavingItemListReq obj = new C2SItemGetSpecifiedHavingItemListReq();
                obj.ItemId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
