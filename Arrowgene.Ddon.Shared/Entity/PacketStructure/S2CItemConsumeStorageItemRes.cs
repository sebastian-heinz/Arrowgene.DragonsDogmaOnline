using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemConsumeStorageItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_CONSUME_STORAGE_ITEM_RES;

        public class Serializer : PacketEntitySerializer<S2CItemConsumeStorageItemRes>
        {
            public override void Write(IBuffer buffer, S2CItemConsumeStorageItemRes obj)
            {
                WriteServerResponse(buffer, obj);
            }
        
            public override S2CItemConsumeStorageItemRes Read(IBuffer buffer)
            {
                S2CItemConsumeStorageItemRes obj = new S2CItemConsumeStorageItemRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}