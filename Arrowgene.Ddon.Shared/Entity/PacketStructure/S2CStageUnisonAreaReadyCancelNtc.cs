using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageUnisonAreaReadyCancelNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_STAGE_UNISON_AREA_READY_CANCEL_NTC;

        public S2CStageUnisonAreaReadyCancelNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CStageUnisonAreaReadyCancelNtc>
        {
            public override void Write(IBuffer buffer, S2CStageUnisonAreaReadyCancelNtc obj)
            {
            }

            public override S2CStageUnisonAreaReadyCancelNtc Read(IBuffer buffer)
            {
                S2CStageUnisonAreaReadyCancelNtc obj = new S2CStageUnisonAreaReadyCancelNtc();
                return obj;
            }
        }
    }
}
