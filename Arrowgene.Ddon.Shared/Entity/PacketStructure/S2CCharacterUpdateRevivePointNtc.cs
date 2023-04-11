using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterUpdateRevivePointNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_UPDATE_REVIVE_POINT_NTC;

        public uint CharacterId { get; set; }
        public byte RevivePoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterUpdateRevivePointNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterUpdateRevivePointNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.RevivePoint);
            }

            public override S2CCharacterUpdateRevivePointNtc Read(IBuffer buffer)
            {
                S2CCharacterUpdateRevivePointNtc obj = new S2CCharacterUpdateRevivePointNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.RevivePoint = ReadByte(buffer);
                return obj;
            }
        }

    }
}