using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetSpecifiedHavingItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_RES;

        public S2CItemGetSpecifiedHavingItemListRes()
        {
            ItemList = new List<CDataItemList>();
        }

        public List<CDataItemList> ItemList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemGetSpecifiedHavingItemListRes>
        {
            public override void Write(IBuffer buffer, S2CItemGetSpecifiedHavingItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ItemList);
            }

            public override S2CItemGetSpecifiedHavingItemListRes Read(IBuffer buffer)
            {
                S2CItemGetSpecifiedHavingItemListRes obj = new S2CItemGetSpecifiedHavingItemListRes();
                ReadServerResponse(buffer, obj);
                obj.ItemList = ReadEntityList<CDataItemList>(buffer);
                return obj;
            }
        }
    }
}
