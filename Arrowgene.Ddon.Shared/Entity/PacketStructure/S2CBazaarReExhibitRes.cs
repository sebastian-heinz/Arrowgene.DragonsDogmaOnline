using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarReExhibitRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_RE_EXHIBIT_RES;

        public ulong BazaarId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarReExhibitRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarReExhibitRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BazaarId);
            }

            public override S2CBazaarReExhibitRes Read(IBuffer buffer)
            {
                S2CBazaarReExhibitRes obj = new S2CBazaarReExhibitRes();
                ReadServerResponse(buffer, obj);
                obj.BazaarId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}