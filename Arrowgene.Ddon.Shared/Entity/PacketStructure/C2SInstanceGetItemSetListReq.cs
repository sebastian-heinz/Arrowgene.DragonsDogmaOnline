using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetItemSetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_ITEM_SET_LIST_REQ;

        public C2SInstanceGetItemSetListReq()
        {
        }

        public CDataStageLayoutId LayoutId { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SInstanceGetItemSetListReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetItemSetListReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
            }

            public override C2SInstanceGetItemSetListReq Read(IBuffer buffer)
            {
                C2SInstanceGetItemSetListReq obj = new C2SInstanceGetItemSetListReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                return obj;
            }
        }
    }
}
