using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetDropItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_DROP_ITEM_LIST_REQ;

        public C2SInstanceGetDropItemListReq()
        {
            LayoutId=new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint SetId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceGetDropItemListReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetDropItemListReq obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.SetId);
            }

            public override C2SInstanceGetDropItemListReq Read(IBuffer buffer)
            {
                C2SInstanceGetDropItemListReq obj = new C2SInstanceGetDropItemListReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SetId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}