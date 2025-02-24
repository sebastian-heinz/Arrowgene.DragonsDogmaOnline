using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetItemSetListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_GET_ITEM_SET_LIST_RES;

        public S2CInstanceGetItemSetListRes()
        {
        }

        public CDataStageLayoutId LayoutId { get; set; } = new();
        public List<CDataLayoutItemData> SetList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CInstanceGetItemSetListRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceGetItemSetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.LayoutId);
                WriteEntityList(buffer, obj.SetList);
            }

            public override S2CInstanceGetItemSetListRes Read(IBuffer buffer)
            {
                S2CInstanceGetItemSetListRes obj = new S2CInstanceGetItemSetListRes();
                ReadServerResponse(buffer, obj);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SetList = ReadEntityList<CDataLayoutItemData>(buffer);
                return obj;
            }
        }
    }
}
