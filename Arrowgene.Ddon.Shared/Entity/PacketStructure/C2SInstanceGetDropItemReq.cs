using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetDropItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_DROP_ITEM_REQ;

        public C2SInstanceGetDropItemReq()
        {
            LayoutId = new CDataStageLayoutId();
            GatheringItemGetRequestList = new List<CDataGatheringItemGetRequest>();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint SetId { get; set; }
        public bool Unk0 { get; set; }
        public List<CDataGatheringItemGetRequest> GatheringItemGetRequestList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceGetDropItemReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetDropItemReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.SetId);
                WriteBool(buffer, obj.Unk0);
                WriteEntityList<CDataGatheringItemGetRequest>(buffer, obj.GatheringItemGetRequestList);
            }

            public override C2SInstanceGetDropItemReq Read(IBuffer buffer)
            {
                C2SInstanceGetDropItemReq obj = new C2SInstanceGetDropItemReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SetId = ReadUInt32(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.GatheringItemGetRequestList = ReadEntityList<CDataGatheringItemGetRequest>(buffer);
                return obj;
            }
        }

    }
}