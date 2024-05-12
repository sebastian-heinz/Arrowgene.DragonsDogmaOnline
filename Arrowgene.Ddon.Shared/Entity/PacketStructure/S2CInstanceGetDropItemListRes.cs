using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetDropItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_GET_DROP_ITEM_LIST_RES;

        public S2CInstanceGetDropItemListRes()
        {
            LayoutId=new CDataStageLayoutId();
            ItemList = new List<CDataGatheringItemElement>();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint SetId { get; set; }
        public List<CDataGatheringItemElement> ItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceGetDropItemListRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceGetDropItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.SetId);
                WriteEntityList<CDataGatheringItemElement>(buffer, obj.ItemList);
            }

            public override S2CInstanceGetDropItemListRes Read(IBuffer buffer)
            {
                S2CInstanceGetDropItemListRes obj = new S2CInstanceGetDropItemListRes();
                ReadServerResponse(buffer, obj);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SetId = ReadUInt32(buffer);
                obj.ItemList = ReadEntityList<CDataGatheringItemElement>(buffer);
                return obj;
            }
        }
    }
}