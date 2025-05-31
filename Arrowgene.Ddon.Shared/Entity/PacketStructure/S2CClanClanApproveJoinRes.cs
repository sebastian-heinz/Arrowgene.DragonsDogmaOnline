using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanApproveJoinRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_APPROVE_JOIN_RES;

        public class Serializer : PacketEntitySerializer<S2CClanClanApproveJoinRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanApproveJoinRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanApproveJoinRes Read(IBuffer buffer)
            {
                S2CClanClanApproveJoinRes obj = new();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
