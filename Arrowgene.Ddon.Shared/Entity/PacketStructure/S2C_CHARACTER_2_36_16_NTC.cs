using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_CHARACTER_2_36_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_2_36_16_NTC;

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2C_CHARACTER_2_36_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_CHARACTER_2_36_16_NTC obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override S2C_CHARACTER_2_36_16_NTC Read(IBuffer buffer)
            {
                S2C_CHARACTER_2_36_16_NTC obj = new S2C_CHARACTER_2_36_16_NTC();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
