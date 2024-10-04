using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetMyJoinRequestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_RES;

        public S2CClanClanGetMyJoinRequestListRes()
        {
            JoinInfo = new List<CDataClanJoinRequest>();
        }

        public List<CDataClanJoinRequest> JoinInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanGetMyJoinRequestListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetMyJoinRequestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanJoinRequest>(buffer, obj.JoinInfo);
            }

            public override S2CClanClanGetMyJoinRequestListRes Read(IBuffer buffer)
            {
                S2CClanClanGetMyJoinRequestListRes obj = new S2CClanClanGetMyJoinRequestListRes();
                ReadServerResponse(buffer, obj);
                obj.JoinInfo = ReadEntityList<CDataClanJoinRequest>(buffer);
                return obj;
            }
        }
    }
}
