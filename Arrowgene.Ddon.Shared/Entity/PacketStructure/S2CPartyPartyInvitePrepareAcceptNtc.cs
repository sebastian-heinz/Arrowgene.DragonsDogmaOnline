using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    // I couldn't find the handler for this in the PC client so this is a guess from the PS4 ver.
    public class S2CPartyPartyInvitePrepareAcceptNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_PREPARE_ACCEPT_NTC;

        public ushort Unk0  { get; set; } 

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInvitePrepareAcceptNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInvitePrepareAcceptNtc obj)
            {
                WriteUInt16(buffer, obj.Unk0);
            }

            public override S2CPartyPartyInvitePrepareAcceptNtc Read(IBuffer buffer)
            {
                return new S2CPartyPartyInvitePrepareAcceptNtc
                {
                    Unk0 = ReadUInt16(buffer)
                };
            }
        }
    }
}