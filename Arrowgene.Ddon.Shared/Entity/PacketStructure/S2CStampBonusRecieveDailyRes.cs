using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStampBonusRecieveDailyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAMP_BONUS_RECIEVE_DAILY_RES;

        public class Serializer : PacketEntitySerializer<S2CStampBonusRecieveDailyRes>
        {
            public override void Write(IBuffer buffer, S2CStampBonusRecieveDailyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CStampBonusRecieveDailyRes Read(IBuffer buffer)
            {
                S2CStampBonusRecieveDailyRes obj = new S2CStampBonusRecieveDailyRes();

                ReadServerResponse(buffer, obj);

                return obj;
            }
        }
    }
}
