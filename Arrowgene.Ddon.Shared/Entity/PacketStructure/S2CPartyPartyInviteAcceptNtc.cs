using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteAcceptNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_ACCEPT_NTC;

        public ushort ServerId { get; set; }
        public uint PartyId { get; set; }
        public uint StageId { get; set; }
        public uint PositionId { get; set; }
        public byte MemberIndex { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteAcceptNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteAcceptNtc obj)
            {
                WriteUInt16(buffer, obj.ServerId);
                WriteUInt32(buffer, obj.PartyId);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.PositionId);
                WriteByte(buffer, obj.MemberIndex);
            }

            public override S2CPartyPartyInviteAcceptNtc Read(IBuffer buffer)
            {
                return new S2CPartyPartyInviteAcceptNtc
                {
                    ServerId = ReadUInt16(buffer),
                    PartyId = ReadUInt32(buffer),
                    StageId = ReadUInt32(buffer),
                    PositionId = ReadUInt32(buffer),
                    MemberIndex = ReadByte(buffer)
                };
            }
        }
    }
}