using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemSellItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_SELL_ITEM_RES;

        public class Serializer : PacketEntitySerializer<S2CItemSellItemRes>
        {
            public override void Write(IBuffer buffer, S2CItemSellItemRes obj)
            {
                WriteServerResponse(buffer, obj);
            }
        
            public override S2CItemSellItemRes Read(IBuffer buffer)
            {
                S2CItemSellItemRes obj = new S2CItemSellItemRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}