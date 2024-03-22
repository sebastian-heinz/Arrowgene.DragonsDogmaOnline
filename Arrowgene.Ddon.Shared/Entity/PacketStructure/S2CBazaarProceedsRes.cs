using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarProceedsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_PROCEEDS_RES;

        public ulong BazaarId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarProceedsRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarProceedsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BazaarId);
            }

            public override S2CBazaarProceedsRes Read(IBuffer buffer)
            {
                S2CBazaarProceedsRes obj = new S2CBazaarProceedsRes();
                ReadServerResponse(buffer, obj);
                obj.BazaarId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}