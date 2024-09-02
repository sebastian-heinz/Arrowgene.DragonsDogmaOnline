using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetCraftProductReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_GET_CRAFT_PRODUCT_REQ;

        public uint CraftMainPawnID { get; set; }
        public StorageType StorageType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftGetCraftProductReq>
        {
            public override void Write(IBuffer buffer, C2SCraftGetCraftProductReq obj)
            {
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteUInt32(buffer, (uint)obj.StorageType);
            }

            public override C2SCraftGetCraftProductReq Read(IBuffer buffer)
            {
                C2SCraftGetCraftProductReq obj = new C2SCraftGetCraftProductReq();

                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.StorageType = (StorageType)ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
