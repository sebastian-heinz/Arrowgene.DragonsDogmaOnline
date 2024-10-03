using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStampBonusRecieveTotalRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAMP_BONUS_RECIEVE_TOTAL_RES;

        public class Serializer : PacketEntitySerializer<S2CStampBonusRecieveTotalRes>
        {
            public override void Write(IBuffer buffer, S2CStampBonusRecieveTotalRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CStampBonusRecieveTotalRes Read(IBuffer buffer)
            {
                S2CStampBonusRecieveTotalRes obj = new S2CStampBonusRecieveTotalRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
