using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetDefaultStorageEmptySlotNumRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_RES;

        public S2CItemGetDefaultStorageEmptySlotNumRes()
        {
            EmptySlotNumList = new List<CDataStorageEmptySlotNum>();
        }

        public List<CDataStorageEmptySlotNum> EmptySlotNumList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemGetDefaultStorageEmptySlotNumRes>
        {
            public override void Write(IBuffer buffer, S2CItemGetDefaultStorageEmptySlotNumRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.EmptySlotNumList);
            }

            public override S2CItemGetDefaultStorageEmptySlotNumRes Read(IBuffer buffer)
            {
                S2CItemGetDefaultStorageEmptySlotNumRes obj = new S2CItemGetDefaultStorageEmptySlotNumRes();
                ReadServerResponse(buffer, obj);
                obj.EmptySlotNumList = ReadEntityList<CDataStorageEmptySlotNum>(buffer);
                return obj;
            }
        }
    }
}
