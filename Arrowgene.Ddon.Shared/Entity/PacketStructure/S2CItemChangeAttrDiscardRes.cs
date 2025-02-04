using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemChangeAttrDiscardRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_CHANGE_ATTR_DISCARD_RES;

        public S2CItemChangeAttrDiscardRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CItemChangeAttrDiscardRes>
        {
            public override void Write(IBuffer buffer, S2CItemChangeAttrDiscardRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CItemChangeAttrDiscardRes Read(IBuffer buffer)
            {
                S2CItemChangeAttrDiscardRes obj = new S2CItemChangeAttrDiscardRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
