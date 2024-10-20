using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanCreateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_CREATE_RES;

        public S2CClanClanCreateRes()
        {
            ClanParam = new CDataClanParam();
            MemberList = new List<CDataClanMemberInfo>();
        }

        public CDataClanParam ClanParam { get; set; }
        public List<CDataClanMemberInfo> MemberList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanCreateRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanCreateRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataClanParam>(buffer, obj.ClanParam);
                WriteEntityList<CDataClanMemberInfo>(buffer, obj.MemberList);
            }

            public override S2CClanClanCreateRes Read(IBuffer buffer)
            {
                S2CClanClanCreateRes obj = new S2CClanClanCreateRes();
                ReadServerResponse(buffer, obj);
                obj.ClanParam = ReadEntity<CDataClanParam>(buffer);
                obj.MemberList = ReadEntityList<CDataClanMemberInfo>(buffer);
                return obj;
            }
        }
    }
}
