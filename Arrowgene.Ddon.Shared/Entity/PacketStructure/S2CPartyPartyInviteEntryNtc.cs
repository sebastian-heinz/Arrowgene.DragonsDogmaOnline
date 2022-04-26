using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteEntryNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_ENTRY_NTC;

        public uint CharacterId { get; set; }
        public uint NowMember { get; set; }
        public uint MaxMember { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteEntryNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteEntryNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.NowMember);
                WriteUInt32(buffer, obj.MaxMember);
            }

            public override S2CPartyPartyInviteEntryNtc Read(IBuffer buffer)
            {
                return new S2CPartyPartyInviteEntryNtc
                {
                    CharacterId = ReadUInt32(buffer),
                    NowMember = ReadUInt32(buffer),
                    MaxMember = ReadUInt32(buffer)
                };
            }
        }
    }
}