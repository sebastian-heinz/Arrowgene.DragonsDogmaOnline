using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceAreaResetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_AREA_RESET_NTC;

        public class Serializer : PacketEntitySerializer<S2CInstanceAreaResetNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceAreaResetNtc obj)
            {
            }

            public override S2CInstanceAreaResetNtc Read(IBuffer buffer)
            {
                return new S2CInstanceAreaResetNtc();
            }
        }
    }
}