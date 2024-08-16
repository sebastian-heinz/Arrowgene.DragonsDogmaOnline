using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetValuableItemListRes : ServerResponse
    {
        public S2CItemGetValuableItemListRes()
        {
            DummyEmptySlotNum = new List<CDataItemStorageIndicateNum>();
            DummyValuableItems = new List<CDataItemStorageIndicateNum>();
        }

        public override PacketId Id => PacketId.S2C_ITEM_GET_VALUABLE_ITEM_LIST_RES;

        public int Unk0 { get; set; }
        public int Unk1 { get; set; }

        //TODO: Implement these CData objects properly.
        //public List<CDataValuableItem> ValuableItems //int, byte, int, short
        //public List<CDataStorageEmptySlotNum> EmptySlotNum //From the BBI PR.
        public List<CDataItemStorageIndicateNum> DummyValuableItems { get; set; }
        public List<CDataItemStorageIndicateNum> DummyEmptySlotNum { get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemGetValuableItemListRes>
        {

            public override void Write(IBuffer buffer, S2CItemGetValuableItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteInt32(buffer, obj.Unk0);
                WriteInt32(buffer, obj.Unk1);
                WriteEntityList<CDataItemStorageIndicateNum>(buffer, obj.DummyValuableItems);
                WriteEntityList<CDataItemStorageIndicateNum>(buffer, obj.DummyEmptySlotNum);
            }

            public override S2CItemGetValuableItemListRes Read(IBuffer buffer)
            {
                S2CItemGetValuableItemListRes obj = new S2CItemGetValuableItemListRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadInt32(buffer);
                obj.Unk1 = ReadInt32(buffer);
                obj.DummyValuableItems = ReadEntityList<CDataItemStorageIndicateNum>(buffer);
                obj.DummyValuableItems = ReadEntityList<CDataItemStorageIndicateNum>(buffer);
                return obj;
            }
        }
    }
}
