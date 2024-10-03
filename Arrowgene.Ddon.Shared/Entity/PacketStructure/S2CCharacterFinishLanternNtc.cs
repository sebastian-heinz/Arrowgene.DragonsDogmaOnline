using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterFinishLanternNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_FINISH_LANTERN_NTC;

        public S2CCharacterFinishLanternNtc()
        {
        }

        public uint RemainTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterFinishLanternNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterFinishLanternNtc obj)
            {
            }

            public override S2CCharacterFinishLanternNtc Read(IBuffer buffer)
            {
                S2CCharacterFinishLanternNtc obj = new S2CCharacterFinishLanternNtc();
                return obj;
            }
        }
    }
}
