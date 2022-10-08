using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyChangeLeaderNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_CHANGE_LEADER_NTC;

        public S2CPartyPartyChangeLeaderNtc()
        {
            CharacterId = 0;
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyChangeLeaderNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyChangeLeaderNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CPartyPartyChangeLeaderNtc Read(IBuffer buffer)
            {
                S2CPartyPartyChangeLeaderNtc obj = new S2CPartyPartyChangeLeaderNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
