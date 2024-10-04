using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanJoinSelfNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_JOIN_SELF_NTC;


        public CDataClanParam ClanParam;
        public CDataClanMemberInfo SelfInfo;
        public List<CDataClanMemberInfo> MemberList;

        public S2CClanClanJoinSelfNtc()
        {
            ClanParam = new();
            SelfInfo = new();
            MemberList = new();
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanJoinSelfNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanJoinSelfNtc obj)
            {
                WriteEntity<CDataClanParam>(buffer, obj.ClanParam);
                WriteEntity<CDataClanMemberInfo>(buffer, obj.SelfInfo);
                WriteEntityList<CDataClanMemberInfo>(buffer, obj.MemberList);
            }

            public override S2CClanClanJoinSelfNtc Read(IBuffer buffer)
            {
                S2CClanClanJoinSelfNtc obj = new S2CClanClanJoinSelfNtc();

                obj.ClanParam = ReadEntity<CDataClanParam>(buffer);
                obj.SelfInfo = ReadEntity<CDataClanMemberInfo>(buffer);
                obj.MemberList = ReadEntityList<CDataClanMemberInfo>(buffer);

                return obj;
            }
        }
    }
}
