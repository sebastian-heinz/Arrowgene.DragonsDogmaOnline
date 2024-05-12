using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyBreakupNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_BREAKUP_NTC;

        public S2CPartyPartyBreakupNtc()
        {
            CharacterId = 0;
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyBreakupNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyBreakupNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CPartyPartyBreakupNtc Read(IBuffer buffer)
            {
                S2CPartyPartyBreakupNtc obj = new S2CPartyPartyBreakupNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
