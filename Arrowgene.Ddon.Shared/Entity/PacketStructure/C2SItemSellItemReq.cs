using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemSellItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_SELL_ITEM_REQ;

        public C2SItemSellItemReq()
        {
            ConsumeItemList = new List<CDataStorageItemUIDList>();
        }

        public List<CDataStorageItemUIDList> ConsumeItemList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SItemSellItemReq>
        {
            public override void Write(IBuffer buffer, C2SItemSellItemReq obj)
            {
                WriteEntityList<CDataStorageItemUIDList>(buffer, obj.ConsumeItemList);
            }

            public override C2SItemSellItemReq Read(IBuffer buffer)
            {
                C2SItemSellItemReq obj = new C2SItemSellItemReq();
                obj.ConsumeItemList = ReadEntityList<CDataStorageItemUIDList>(buffer);
                return obj;
            }
        }

    }
}