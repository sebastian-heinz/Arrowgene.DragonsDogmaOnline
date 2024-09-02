using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextMasterThrowRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONTEXT_MASTER_THROW_RES;

        public class Serializer : PacketEntitySerializer<S2CContextMasterThrowRes>
        {
            public override void Write(IBuffer buffer, S2CContextMasterThrowRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CContextMasterThrowRes Read(IBuffer buffer)
            {
                S2CContextMasterThrowRes obj = new S2CContextMasterThrowRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}