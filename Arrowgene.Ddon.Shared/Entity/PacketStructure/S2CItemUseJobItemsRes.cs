using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemUseJobItemsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_USE_JOB_ITEMS_RES;

        public S2CItemUseJobItemsRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CItemUseJobItemsRes>
        {
            public override void Write(IBuffer buffer, S2CItemUseJobItemsRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CItemUseJobItemsRes Read(IBuffer buffer)
            {
                S2CItemUseJobItemsRes obj = new S2CItemUseJobItemsRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}