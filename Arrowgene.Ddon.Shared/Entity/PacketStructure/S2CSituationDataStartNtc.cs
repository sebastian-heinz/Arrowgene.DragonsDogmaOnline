using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSituationDataStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_63_0_16_NTC;

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSituationDataStartNtc>
        {
            public override void Write(IBuffer buffer, S2CSituationDataStartNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override S2CSituationDataStartNtc Read(IBuffer buffer)
            {
                S2CSituationDataStartNtc obj = new S2CSituationDataStartNtc();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
