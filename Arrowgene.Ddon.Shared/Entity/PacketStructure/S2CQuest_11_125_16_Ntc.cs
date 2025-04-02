using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuest_11_125_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_11_125_16_NTC;

        public S2CQuest_11_125_16_Ntc()
        {
        }

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuest_11_125_16_Ntc>
        {
            public override void Write(IBuffer buffer, S2CQuest_11_125_16_Ntc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override S2CQuest_11_125_16_Ntc Read(IBuffer buffer)
            {
                S2CQuest_11_125_16_Ntc obj = new S2CQuest_11_125_16_Ntc();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

