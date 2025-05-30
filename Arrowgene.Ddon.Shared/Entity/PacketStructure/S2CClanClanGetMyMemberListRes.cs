using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetMyMemberListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_MY_MEMBER_LIST_RES;

        public S2CClanClanGetMyMemberListRes()
        {
            MemberList = new List<CDataClanMemberInfo>();
        }

        public List<CDataClanMemberInfo> MemberList;

        public class Serializer : PacketEntitySerializer<S2CClanClanGetMyMemberListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetMyMemberListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanMemberInfo>(buffer, obj.MemberList);
            }

            public override S2CClanClanGetMyMemberListRes Read(IBuffer buffer)
            {
                S2CClanClanGetMyMemberListRes obj = new S2CClanClanGetMyMemberListRes();
                ReadServerResponse(buffer, obj);
                obj.MemberList = ReadEntityList<CDataClanMemberInfo>(buffer);
                return obj;
            }
        }
    }
}
