using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterStartLanternNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_START_LANTERN_NTC;

        public S2CCharacterStartLanternNtc()
        {
        }

        public uint RemainTime { get; set; } // Time in seconds (pcap has 25 minutes)

        public class Serializer : PacketEntitySerializer<S2CCharacterStartLanternNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterStartLanternNtc obj)
            {
                WriteUInt32(buffer, obj.RemainTime);
            }

            public override S2CCharacterStartLanternNtc Read(IBuffer buffer)
            {
                S2CCharacterStartLanternNtc obj = new S2CCharacterStartLanternNtc();
                obj.RemainTime = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
