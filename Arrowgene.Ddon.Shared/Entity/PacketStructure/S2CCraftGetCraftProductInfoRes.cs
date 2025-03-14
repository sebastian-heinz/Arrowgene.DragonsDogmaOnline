using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftGetCraftProductInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_GET_CRAFT_PRODUCT_INFO_RES;

        public CDataCraftProductInfo CraftProductInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CCraftGetCraftProductInfoRes>
        {
            public override void Write(IBuffer buffer, S2CCraftGetCraftProductInfoRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntity(buffer, obj.CraftProductInfo);
            }

            public override S2CCraftGetCraftProductInfoRes Read(IBuffer buffer)
            {
                S2CCraftGetCraftProductInfoRes obj = new S2CCraftGetCraftProductInfoRes();

                ReadServerResponse(buffer, obj);

                obj.CraftProductInfo = ReadEntity<CDataCraftProductInfo>(buffer);

                return obj;
            }
        }
    }
}
