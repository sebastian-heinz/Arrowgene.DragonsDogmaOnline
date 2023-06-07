using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;


namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetGatheringItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_GET_GATHERING_ITEM_RES;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public List<CDataGatheringItemGetRequest> GatheringItemGetRequestList { get; set; }

        public S2CInstanceGetGatheringItemRes()
        {
            LayoutId = new CDataStageLayoutId();
            GatheringItemGetRequestList = new List<CDataGatheringItemGetRequest>();
        }

        public class Serializer : PacketEntitySerializer<S2CInstanceGetGatheringItemRes>
        {

            public override void Write(IBuffer buffer, S2CInstanceGetGatheringItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteEntityList<CDataGatheringItemGetRequest>(buffer, obj.GatheringItemGetRequestList);
            }

            public override S2CInstanceGetGatheringItemRes Read(IBuffer buffer)
            {
                S2CInstanceGetGatheringItemRes obj = new S2CInstanceGetGatheringItemRes();
                ReadServerResponse(buffer, obj);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.GatheringItemGetRequestList = ReadEntityList<CDataGatheringItemGetRequest>(buffer);
                return obj;
            }

        }
    }
}
