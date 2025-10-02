using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetValuableItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_VALUABLE_ITEM_LIST_RES;

        public List<CDataValuableItem> ValuableItems { get; set; } = [];
        public List<CDataStorageEmptySlotNum> EmptySlotNumList { get; set; } = [];

        public class Serializer : PacketEntitySerializer<S2CItemGetValuableItemListRes>
        {

            public override void Write(IBuffer buffer, S2CItemGetValuableItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ValuableItems);
                WriteEntityList(buffer, obj.EmptySlotNumList);
            }

            public override S2CItemGetValuableItemListRes Read(IBuffer buffer)
            {
                S2CItemGetValuableItemListRes obj = new S2CItemGetValuableItemListRes();
                ReadServerResponse(buffer, obj);
                obj.ValuableItems = ReadEntityList<CDataValuableItem>(buffer);
                obj.EmptySlotNumList = ReadEntityList<CDataStorageEmptySlotNum>(buffer);
                return obj;
            }
        }
    }
}
