using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_SEASON_62_38_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SEASON_62_38_16_NTC;

        public S2C_SEASON_62_38_16_NTC()
        {
        }

        public uint StageNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2C_SEASON_62_38_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_SEASON_62_38_16_NTC obj)
            {
            }

            public override S2C_SEASON_62_38_16_NTC Read(IBuffer buffer)
            {
                S2C_SEASON_62_38_16_NTC obj = new S2C_SEASON_62_38_16_NTC();
                return obj;
            }
        }
    }
}
