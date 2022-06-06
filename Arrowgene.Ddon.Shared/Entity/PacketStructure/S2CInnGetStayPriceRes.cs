using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInnGetStayPriceRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INN_GET_STAY_PRICE_RES;

        public S2CInnGetStayPriceRes()
        {
        }

        public C2SInnGetStayPriceReq ReqData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInnGetStayPriceRes>
        {
            public override void Write(IBuffer buffer, S2CInnGetStayPriceRes obj)
            {
                C2SInnGetStayPriceReq req = obj.ReqData;
                WriteUInt64(buffer, 0);
                WriteByte(buffer, 1);
                WriteUInt32(buffer, 100);
            }

            public override S2CInnGetStayPriceRes Read(IBuffer buffer)
            {
                S2CInnGetStayPriceRes obj = new S2CInnGetStayPriceRes();
                return obj;
            }
        }

    }
}
