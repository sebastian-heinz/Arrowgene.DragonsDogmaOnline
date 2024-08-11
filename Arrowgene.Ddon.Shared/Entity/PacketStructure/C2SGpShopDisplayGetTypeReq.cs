using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpShopDisplayGetTypeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GP_SHOP_DISPLAY_GET_TYPE_REQ;

        public C2SGpShopDisplayGetTypeReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGpShopDisplayGetTypeReq>
        {
            public override void Write(IBuffer buffer, C2SGpShopDisplayGetTypeReq obj)
            {
            }

            public override C2SGpShopDisplayGetTypeReq Read(IBuffer buffer)
            {
                C2SGpShopDisplayGetTypeReq obj = new C2SGpShopDisplayGetTypeReq();

                return obj;
            }
        }
    }
}
