using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SContext_35_5_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONTEXT_35_5_16_NTC;

        public C2SContext_35_5_16_Ntc()
        {
        }

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SContext_35_5_16_Ntc>
        {
            public override void Write(IBuffer buffer, C2SContext_35_5_16_Ntc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SContext_35_5_16_Ntc Read(IBuffer buffer)
            {
                C2SContext_35_5_16_Ntc obj = new C2SContext_35_5_16_Ntc();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
