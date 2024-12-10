using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetJoinRequestedListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_RES;

        public S2CClanClanGetJoinRequestedListRes()
        {
            JoinReqList = new List<CDataClanJoinRequest>();
        }

        public List<CDataClanJoinRequest> JoinReqList;

        public class Serializer : PacketEntitySerializer<S2CClanClanGetJoinRequestedListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetJoinRequestedListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanJoinRequest>(buffer, obj.JoinReqList);
            }

            public override S2CClanClanGetJoinRequestedListRes Read(IBuffer buffer)
            {
                S2CClanClanGetJoinRequestedListRes obj = new S2CClanClanGetJoinRequestedListRes();
                ReadServerResponse(buffer, obj);
                obj.JoinReqList = ReadEntityList<CDataClanJoinRequest>(buffer);
                return obj;
            }
        }
    }
}
