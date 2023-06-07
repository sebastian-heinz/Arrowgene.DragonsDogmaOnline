using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemMoveItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_MOVE_ITEM_RES;

        public class Serializer : PacketEntitySerializer<S2CItemMoveItemRes>
        {
            public override void Write(IBuffer buffer, S2CItemMoveItemRes obj)
            {
                WriteServerResponse(buffer, obj);
            }
        
            public override S2CItemMoveItemRes Read(IBuffer buffer)
            {
                S2CItemMoveItemRes obj = new S2CItemMoveItemRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}