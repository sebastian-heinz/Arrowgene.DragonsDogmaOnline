using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextGetSetContextRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONTEXT_GET_SET_CONTEXT_RES;

        public class Serializer : PacketEntitySerializer<S2CContextGetSetContextRes>
        {
            public override void Write(IBuffer buffer, S2CContextGetSetContextRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CContextGetSetContextRes Read(IBuffer buffer)
            {
                S2CContextGetSetContextRes obj = new S2CContextGetSetContextRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
