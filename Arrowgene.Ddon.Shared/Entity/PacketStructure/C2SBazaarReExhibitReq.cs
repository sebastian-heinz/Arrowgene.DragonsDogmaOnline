using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarReExhibitReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_RE_EXHIBIT_REQ;

        public ulong BazaarId { get; set; }
        public uint Price { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarReExhibitReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarReExhibitReq obj)
            {
                WriteUInt64(buffer, obj.BazaarId);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SBazaarReExhibitReq Read(IBuffer buffer)
            {
                C2SBazaarReExhibitReq obj = new C2SBazaarReExhibitReq();
                obj.BazaarId = ReadUInt64(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}