using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarGetItemInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_GET_ITEM_INFO_REQ;

        public uint ItemId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarGetItemInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarGetItemInfoReq obj)
            {
                WriteUInt32(buffer, obj.ItemId);
            }

            public override C2SBazaarGetItemInfoReq Read(IBuffer buffer)
            {
                C2SBazaarGetItemInfoReq obj = new C2SBazaarGetItemInfoReq();
                obj.ItemId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}