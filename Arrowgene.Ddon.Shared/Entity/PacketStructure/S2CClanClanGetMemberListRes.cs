using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetMemberListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_MEMBER_LIST_RES;

        public S2CClanClanGetMemberListRes()
        {
            MemberList = new List<CDataClanMemberInfo>();
        }

        public List<CDataClanMemberInfo> MemberList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanGetMemberListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetMemberListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanMemberInfo>(buffer, obj.MemberList);
            }

            public override S2CClanClanGetMemberListRes Read(IBuffer buffer)
            {
                S2CClanClanGetMemberListRes obj = new S2CClanClanGetMemberListRes();
                ReadServerResponse(buffer, obj);
                obj.MemberList = ReadEntityList<CDataClanMemberInfo>(buffer);
                return obj;
            }
        }
    }
}
