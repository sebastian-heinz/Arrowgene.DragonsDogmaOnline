using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetDropItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_GET_DROP_ITEM_RES;

        public S2CInstanceGetDropItemRes()
        {
            LayoutId = new CDataStageLayoutId();
            GatheringItemGetRequestList = new List<CDataGatheringItemGetRequest>();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint SetId { get; set; }
        public List<CDataGatheringItemGetRequest> GatheringItemGetRequestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceGetDropItemRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceGetDropItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.SetId);
                WriteEntityList<CDataGatheringItemGetRequest>(buffer, obj.GatheringItemGetRequestList);
            }

            public override S2CInstanceGetDropItemRes Read(IBuffer buffer)
            {
                S2CInstanceGetDropItemRes obj = new S2CInstanceGetDropItemRes();
                ReadServerResponse(buffer, obj);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SetId = ReadUInt32(buffer);
                obj.GatheringItemGetRequestList = ReadEntityList<CDataGatheringItemGetRequest>(buffer);
                return obj;
            }
        }
    }
}