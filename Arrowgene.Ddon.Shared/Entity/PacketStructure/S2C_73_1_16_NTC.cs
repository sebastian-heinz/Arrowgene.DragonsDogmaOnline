using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_73_1_16_NTC : IPacketStructure
    {
        public S2C_73_1_16_NTC()
        {
        }

        public uint PartyDragonPoints {  get; set; }

        public PacketId Id => PacketId.S2C_73_1_16_NTC;

        public class Serializer : PacketEntitySerializer<S2C_73_1_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_73_1_16_NTC obj)
            {
                WriteUInt32(buffer, obj.PartyDragonPoints);
            }

            public override S2C_73_1_16_NTC Read(IBuffer buffer)
            {
                var obj = new S2C_73_1_16_NTC();
                obj.PartyDragonPoints = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
