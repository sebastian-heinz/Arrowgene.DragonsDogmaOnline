using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CGpShopGetBuyHistoryRes : ServerResponse
{
    public S2CGpShopGetBuyHistoryRes()
    {
        Items = new List<CDataGPShopBuyHistoryElement>();
    }

    public override PacketId Id => PacketId.S2C_GP_GP_SHOP_GET_BUY_HISTORY_RES;

    public List<CDataGPShopBuyHistoryElement> Items { get; set; }

    public class Serializer : PacketEntitySerializer<S2CGpShopGetBuyHistoryRes>
    {
        public override void Write(IBuffer buffer, S2CGpShopGetBuyHistoryRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.Items);
        }

        public override S2CGpShopGetBuyHistoryRes Read(IBuffer buffer)
        {
            var obj = new S2CGpShopGetBuyHistoryRes();

            ReadServerResponse(buffer, obj);

            obj.Items = ReadEntityList<CDataGPShopBuyHistoryElement>(buffer);

            return obj;
        }
    }
}
