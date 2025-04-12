using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyChangeHostNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_CHANGE_HOST_NTC;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyChangeHostNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyChangeHostNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CPartyChangeHostNtc Read(IBuffer buffer)
            {
                S2CPartyChangeHostNtc obj = new S2CPartyChangeHostNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
