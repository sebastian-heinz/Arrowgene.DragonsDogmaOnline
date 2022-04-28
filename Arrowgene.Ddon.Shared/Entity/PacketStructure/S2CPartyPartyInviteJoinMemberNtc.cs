using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteJoinMemberNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_JOIN_MEMBER_NTC;

        public S2CPartyPartyInviteJoinMemberNtc()
        {
            MemberMinimumList = new List<CDataPartyMemberMinimum>();
        }

        public List<CDataPartyMemberMinimum> MemberMinimumList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteJoinMemberNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteJoinMemberNtc obj)
            {
                WriteEntityList<CDataPartyMemberMinimum>(buffer, obj.MemberMinimumList);
            }

            public override S2CPartyPartyInviteJoinMemberNtc Read(IBuffer buffer)
            {
                S2CPartyPartyInviteJoinMemberNtc packet = new S2CPartyPartyInviteJoinMemberNtc();
                packet.MemberMinimumList = ReadEntityList<CDataPartyMemberMinimum>(buffer);
                return packet;
            }
        }
    }
}