using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpShopDisplayGetTypeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GP_SHOP_DISPLAY_GET_TYPE_RES;

        public List<CDataGPShopDisplayType> Items { get; set; }

        public S2CGpShopDisplayGetTypeRes()
        {
            Items = new List<CDataGPShopDisplayType>();
        }

        public class Serializer : PacketEntitySerializer<S2CGpShopDisplayGetTypeRes>
        {
            public override void Write(IBuffer buffer, S2CGpShopDisplayGetTypeRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList(buffer, obj.Items);
            }

            public override S2CGpShopDisplayGetTypeRes Read(IBuffer buffer)
            {
                S2CGpShopDisplayGetTypeRes obj = new S2CGpShopDisplayGetTypeRes();

                ReadServerResponse(buffer, obj);

                obj.Items = ReadEntityList<CDataGPShopDisplayType>(buffer);

                return obj;
            }
        }
    }
}
