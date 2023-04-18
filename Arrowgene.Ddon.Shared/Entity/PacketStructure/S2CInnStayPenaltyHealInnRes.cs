using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInnStayPenaltyHealInnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INN_STAY_PENALTY_HEAL_INN_RES;

        public WalletType PointType { get; set; }
        public uint Point { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInnStayPenaltyHealInnRes>
        {
            public override void Write(IBuffer buffer, S2CInnStayPenaltyHealInnRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.PointType);
                WriteUInt32(buffer, obj.Point);
            }

            public override S2CInnStayPenaltyHealInnRes Read(IBuffer buffer)
            {
                S2CInnStayPenaltyHealInnRes obj = new S2CInnStayPenaltyHealInnRes();
                ReadServerResponse(buffer, obj);
                obj.PointType = (WalletType) ReadByte(buffer);
                obj.Point = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}