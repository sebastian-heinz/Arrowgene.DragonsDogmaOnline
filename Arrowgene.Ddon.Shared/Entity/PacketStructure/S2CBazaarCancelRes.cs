using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarCancelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_CANCEL_RES;

        public ulong BazaarId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarCancelRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarCancelRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BazaarId);
            }

            public override S2CBazaarCancelRes Read(IBuffer buffer)
            {
                S2CBazaarCancelRes obj = new S2CBazaarCancelRes();
                ReadServerResponse(buffer, obj);
                obj.BazaarId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}