using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterChargeRevivePointRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_CHARGE_REVIVE_POINT_RES;

        public byte RevivePoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterChargeRevivePointRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterChargeRevivePointRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.RevivePoint);
            }

            public override S2CCharacterChargeRevivePointRes Read(IBuffer buffer)
            {
                S2CCharacterChargeRevivePointRes obj = new S2CCharacterChargeRevivePointRes();
                ReadServerResponse(buffer, obj);
                obj.RevivePoint = ReadByte(buffer);
                return obj;
            }
        }
    }
}