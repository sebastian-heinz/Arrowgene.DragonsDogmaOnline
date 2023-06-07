using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetStorageItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_STORAGE_ITEM_LIST_RES;

        public S2CItemGetStorageItemListRes()
        {
            ItemList = new List<CDataItemList>();
        }

        public List<CDataItemList> ItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemGetStorageItemListRes>
        {
            public override void Write(IBuffer buffer, S2CItemGetStorageItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataItemList>(buffer, obj.ItemList);
            }

            public override S2CItemGetStorageItemListRes Read(IBuffer buffer)
            {
                S2CItemGetStorageItemListRes obj = new S2CItemGetStorageItemListRes();
                ReadServerResponse(buffer, obj);
                obj.ItemList = ReadEntityList<CDataItemList>(buffer);
                return obj;
            }
        }
    }
}