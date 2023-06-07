using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInnGetPenaltyHealStayPriceRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INN_GET_PENALTY_HEAL_STAY_PRICE_RES;

        public WalletType PointType { get; set; }
        public uint Point { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInnGetPenaltyHealStayPriceRes>
        {
            public override void Write(IBuffer buffer, S2CInnGetPenaltyHealStayPriceRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.PointType);
                WriteUInt32(buffer, obj.Point);
            }

            public override S2CInnGetPenaltyHealStayPriceRes Read(IBuffer buffer)
            {
                S2CInnGetPenaltyHealStayPriceRes obj = new S2CInnGetPenaltyHealStayPriceRes();
                ReadServerResponse(buffer, obj);
                obj.PointType = (WalletType) ReadByte(buffer);
                obj.Point = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}