using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;


namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetGatheringItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_GET_GATHERING_ITEM_LIST_RES;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public string GatheringItemUId { get; set; }
        public bool IsGatheringItemBreak { get; set; }
        public bool Unk0 { get; set; }
        public List<CDataGatheringItemListUnk1> Unk1 { get; set; } // Currencies?
        public List<CDataGatheringItemElement> ItemList { get; set; } // Items
        
        public S2CInstanceGetGatheringItemListRes()
        {
            LayoutId = new CDataStageLayoutId();
            Unk1 = new List<CDataGatheringItemListUnk1>();
            ItemList = new List<CDataGatheringItemElement>();
            GatheringItemUId = string.Empty;
        }

        public class Serializer : PacketEntitySerializer<S2CInstanceGetGatheringItemListRes>
        {

            public override void Write(IBuffer buffer, S2CInstanceGetGatheringItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteMtString(buffer, obj.GatheringItemUId);
                WriteBool(buffer, obj.IsGatheringItemBreak);
                WriteBool(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.ItemList);
            }

            public override S2CInstanceGetGatheringItemListRes Read(IBuffer buffer)
            {
                S2CInstanceGetGatheringItemListRes obj = new S2CInstanceGetGatheringItemListRes();
                ReadServerResponse(buffer, obj);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.GatheringItemUId = ReadMtString(buffer);
                obj.IsGatheringItemBreak = ReadBool(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadEntityList<CDataGatheringItemListUnk1>(buffer);
                obj.ItemList = ReadEntityList<CDataGatheringItemElement>(buffer);
                return obj;
            }

        }
    }
}
