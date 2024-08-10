using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetCraftProductInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_GET_CRAFT_PRODUCT_INFO_REQ;

        public uint CraftMainPawnID { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftGetCraftProductInfoReq>
        {
            public override void Write(IBuffer buffer, C2SCraftGetCraftProductInfoReq obj)
            {
                WriteUInt32(buffer, obj.CraftMainPawnID);
            }

            public override C2SCraftGetCraftProductInfoReq Read(IBuffer buffer)
            {
                C2SCraftGetCraftProductInfoReq obj = new C2SCraftGetCraftProductInfoReq();

                obj.CraftMainPawnID = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
