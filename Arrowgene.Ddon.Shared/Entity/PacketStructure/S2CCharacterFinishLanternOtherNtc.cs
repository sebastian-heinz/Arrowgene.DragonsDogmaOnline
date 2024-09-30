using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterFinishLanternOtherNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_FINISH_LANTERN_OTHER_NTC;

        public S2CCharacterFinishLanternOtherNtc()
        {
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterFinishLanternOtherNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterFinishLanternOtherNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CCharacterFinishLanternOtherNtc Read(IBuffer buffer)
            {
                S2CCharacterFinishLanternOtherNtc obj = new S2CCharacterFinishLanternOtherNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
