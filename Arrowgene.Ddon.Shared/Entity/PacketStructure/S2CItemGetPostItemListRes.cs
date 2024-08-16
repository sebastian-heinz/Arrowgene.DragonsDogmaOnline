using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetPostItemListRes : ServerResponse
    {
        public S2CItemGetPostItemListRes()
        {
            ItemList = new List<CDataItemList>();
        }

        public override PacketId Id => PacketId.S2C_ITEM_GET_POST_ITEM_LIST_RES;

        public List<CDataItemList> ItemList;

        public class Serializer : PacketEntitySerializer<S2CItemGetPostItemListRes>
        {

            public override void Write(IBuffer buffer, S2CItemGetPostItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataItemList>(buffer, obj.ItemList);
            }

            public override S2CItemGetPostItemListRes Read(IBuffer buffer)
            {
                S2CItemGetPostItemListRes obj = new S2CItemGetPostItemListRes();
                ReadServerResponse(buffer, obj);
                obj.ItemList = ReadEntityList<CDataItemList>(buffer);
                return obj;
            }
        }
    }
}
