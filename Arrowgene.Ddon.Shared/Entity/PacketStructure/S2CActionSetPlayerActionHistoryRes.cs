using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CActionSetPlayerActionHistoryRes
    {
    }

    public class S2CActionSetPlayerActionHistoryResSerializer : EntitySerializer<S2CActionSetPlayerActionHistoryRes>
    {
        public override void Write(IBuffer buffer, S2CActionSetPlayerActionHistoryRes obj)
        {
        }

        public override S2CActionSetPlayerActionHistoryRes Read(IBuffer buffer)
        {
            return new S2CActionSetPlayerActionHistoryRes();
        }
    }
}