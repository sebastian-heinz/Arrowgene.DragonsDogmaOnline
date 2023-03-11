using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterGetReviveChargeableTimeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_RES;

        public uint RemainTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterGetReviveChargeableTimeRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterGetReviveChargeableTimeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.RemainTime);
            }

            public override S2CCharacterGetReviveChargeableTimeRes Read(IBuffer buffer)
            {
                S2CCharacterGetReviveChargeableTimeRes obj = new S2CCharacterGetReviveChargeableTimeRes();
                ReadServerResponse(buffer, obj);
                obj.RemainTime = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}