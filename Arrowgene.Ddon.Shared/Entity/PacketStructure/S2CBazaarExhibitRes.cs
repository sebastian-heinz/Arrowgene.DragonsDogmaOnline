using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarExhibitRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_EXHIBIT_RES;

        public ulong BazaarId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarExhibitRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarExhibitRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BazaarId);
            }

            public override S2CBazaarExhibitRes Read(IBuffer buffer)
            {
                S2CBazaarExhibitRes obj = new S2CBazaarExhibitRes();
                ReadServerResponse(buffer, obj);
                obj.BazaarId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}