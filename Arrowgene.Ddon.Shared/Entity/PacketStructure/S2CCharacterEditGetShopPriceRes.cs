using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterEditGetShopPriceRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_EDIT_GET_SHOP_PRICE_RES;
        public S2CCharacterEditGetShopPriceRes()
        {
            PriceInfo = new List<CDataCharacterEditPriceInfo>();
        }

        public List<CDataCharacterEditPriceInfo> PriceInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterEditGetShopPriceRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterEditGetShopPriceRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.PriceInfo);
            }

            public override S2CCharacterEditGetShopPriceRes Read(IBuffer buffer)
            {
                S2CCharacterEditGetShopPriceRes obj = new S2CCharacterEditGetShopPriceRes();
                ReadServerResponse(buffer, obj);
                obj.PriceInfo = ReadEntityList< CDataCharacterEditPriceInfo>(buffer);
                return obj;
            }
        }
    }
}
