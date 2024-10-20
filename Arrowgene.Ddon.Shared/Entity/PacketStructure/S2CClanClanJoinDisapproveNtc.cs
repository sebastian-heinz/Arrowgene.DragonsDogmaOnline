using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanJoinDisapproveNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_JOIN_DISAPPROVE_NTC;

        public CDataClanJoinRequest JoinReq;

        public S2CClanClanJoinDisapproveNtc()
        {
            JoinReq = new();
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanJoinDisapproveNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanJoinDisapproveNtc obj)
            {
                WriteEntity<CDataClanJoinRequest>(buffer, obj.JoinReq);
            }

            public override S2CClanClanJoinDisapproveNtc Read(IBuffer buffer)
            {
                S2CClanClanJoinDisapproveNtc obj = new S2CClanClanJoinDisapproveNtc();

                obj.JoinReq = ReadEntity<CDataClanJoinRequest>(buffer);

                return obj;
            }
        }
    }
}
