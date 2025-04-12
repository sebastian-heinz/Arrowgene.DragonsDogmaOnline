using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CRecycleResetCountRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_RECYCLE_RESET_COUNT_RES;

        public S2CRecycleResetCountRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CRecycleResetCountRes>
        {
            public override void Write(IBuffer buffer, S2CRecycleResetCountRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CRecycleResetCountRes Read(IBuffer buffer)
            {
                S2CRecycleResetCountRes obj = new S2CRecycleResetCountRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}

