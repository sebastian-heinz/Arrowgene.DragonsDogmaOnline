using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstance_13_42_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_13_42_16_NTC;

        public class Serializer : PacketEntitySerializer<S2CInstance_13_42_16_Ntc>
        {
            public override void Write(IBuffer buffer, S2CInstance_13_42_16_Ntc obj)
            {
            }

            public override S2CInstance_13_42_16_Ntc Read(IBuffer buffer)
            {
                return new S2CInstance_13_42_16_Ntc();
            }
        }
    }
}