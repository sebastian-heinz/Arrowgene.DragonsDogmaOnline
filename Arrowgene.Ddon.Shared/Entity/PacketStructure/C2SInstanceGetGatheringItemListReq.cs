using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetGatheringItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_GATHERING_ITEM_LIST_REQ;
        
        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public string GatheringItemUId { get; set; }

        public C2SInstanceGetGatheringItemListReq()
        {
            LayoutId = new CDataStageLayoutId();
            GatheringItemUId = string.Empty;
        }

        public class Serializer : PacketEntitySerializer<C2SInstanceGetGatheringItemListReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetGatheringItemListReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteMtString(buffer, obj.GatheringItemUId);
            }
            public override C2SInstanceGetGatheringItemListReq Read(IBuffer buffer)
            {
                C2SInstanceGetGatheringItemListReq obj = new C2SInstanceGetGatheringItemListReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.GatheringItemUId = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
