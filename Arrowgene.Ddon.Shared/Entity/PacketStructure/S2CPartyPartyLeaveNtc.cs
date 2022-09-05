using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyLeaveNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_LEAVE_NTC;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyLeaveNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyLeaveNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CPartyPartyLeaveNtc Read(IBuffer buffer)
            {
                S2CPartyPartyLeaveNtc partyLeaveNtc = new S2CPartyPartyLeaveNtc();
                partyLeaveNtc.CharacterId = ReadUInt32(buffer);
                return partyLeaveNtc;
            }
        }
    }
}