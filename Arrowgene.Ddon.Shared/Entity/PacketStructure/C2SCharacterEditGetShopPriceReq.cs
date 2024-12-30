using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterEditGetShopPriceReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_EDIT_GET_SHOP_PRICE_REQ;

        public class Serializer : PacketEntitySerializer<C2SCharacterEditGetShopPriceReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterEditGetShopPriceReq obj)
            {
            }

            public override C2SCharacterEditGetShopPriceReq Read(IBuffer buffer)
            {
                C2SCharacterEditGetShopPriceReq obj = new C2SCharacterEditGetShopPriceReq();
                return obj;
            }
        }
    }
}
