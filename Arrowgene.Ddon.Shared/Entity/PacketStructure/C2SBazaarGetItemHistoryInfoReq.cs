using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarGetItemHistoryInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_GET_ITEM_HISTORY_INFO_REQ;

        public uint ItemId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarGetItemHistoryInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarGetItemHistoryInfoReq obj)
            {
                WriteUInt32(buffer, obj.ItemId);
            }

            public override C2SBazaarGetItemHistoryInfoReq Read(IBuffer buffer)
            {
                C2SBazaarGetItemHistoryInfoReq obj = new C2SBazaarGetItemHistoryInfoReq();
                obj.ItemId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}