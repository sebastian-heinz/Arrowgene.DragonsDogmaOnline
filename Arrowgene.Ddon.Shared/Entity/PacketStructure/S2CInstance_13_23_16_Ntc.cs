using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstance_13_23_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_13_23_16_NTC;

        public S2CInstance_13_23_16_Ntc()
        {
            StageId = 0;
            Data0 = 0;
            Data1 = 0;
        }

        public uint StageId { get; set; }
        public ulong Data0 { get; set; }
        public uint Data1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstance_13_23_16_Ntc>
        {
            public override void Write(IBuffer buffer, S2CInstance_13_23_16_Ntc obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteUInt64(buffer, obj.Data0);
                WriteUInt32(buffer, obj.Data1);
                WriteByteArray(buffer, NtcData);
            }

            public override S2CInstance_13_23_16_Ntc Read(IBuffer buffer)
            {
                S2CInstance_13_23_16_Ntc obj = new S2CInstance_13_23_16_Ntc();
                return obj;
            }
            private readonly byte[] NtcData = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        }

    }
}
