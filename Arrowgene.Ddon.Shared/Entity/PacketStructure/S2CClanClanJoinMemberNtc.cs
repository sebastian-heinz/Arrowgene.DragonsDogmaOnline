using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanJoinMemberNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_JOIN_MEMBER_NTC;

        public uint ClanId;
        public CDataClanMemberInfo MemberInfo;

        public S2CClanClanJoinMemberNtc()
        {
            MemberInfo = new CDataClanMemberInfo();
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanJoinMemberNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanJoinMemberNtc obj)
            {
                WriteUInt32(buffer, obj.ClanId);
                WriteEntity<CDataClanMemberInfo>(buffer, obj.MemberInfo);
            }

            public override S2CClanClanJoinMemberNtc Read(IBuffer buffer)
            {
                S2CClanClanJoinMemberNtc obj = new S2CClanClanJoinMemberNtc();

                obj.ClanId = ReadUInt32(buffer);
                obj.MemberInfo = ReadEntity<CDataClanMemberInfo>(buffer);

                return obj;
            }
        }
    }
}
