using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnection_10_Ntc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_0_10_16_NTC;

        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnection_10_Ntc>
        {
            public override void Write(IBuffer buffer, S2CConnection_10_Ntc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override S2CConnection_10_Ntc Read(IBuffer buffer)
            {
                S2CConnection_10_Ntc obj = new S2CConnection_10_Ntc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
