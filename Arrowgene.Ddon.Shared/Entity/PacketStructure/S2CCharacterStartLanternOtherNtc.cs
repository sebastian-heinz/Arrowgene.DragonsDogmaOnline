using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterStartLanternOtherNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_START_LANTERN_OTHER_NTC;

        public S2CCharacterStartLanternOtherNtc()
        {
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterStartLanternOtherNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterStartLanternOtherNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CCharacterStartLanternOtherNtc Read(IBuffer buffer)
            {
                S2CCharacterStartLanternOtherNtc obj = new S2CCharacterStartLanternOtherNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
