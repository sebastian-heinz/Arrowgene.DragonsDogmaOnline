using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetCraftProductInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_GET_CRAFT_PRODUCT_INFO_RES;

        public CDataCraftProductInfo CraftProductInfo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftGetCraftProductInfoRes>
        {
            public override void Write(IBuffer buffer, C2SCraftGetCraftProductInfoRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntity(buffer, obj.CraftProductInfo);
            }

            public override C2SCraftGetCraftProductInfoRes Read(IBuffer buffer)
            {
                C2SCraftGetCraftProductInfoRes obj = new C2SCraftGetCraftProductInfoRes();

                ReadServerResponse(buffer, obj);

                obj.CraftProductInfo = ReadEntity<CDataCraftProductInfo>(buffer);

                return obj;
            }
        }
    }
}
