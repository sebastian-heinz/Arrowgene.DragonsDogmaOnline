using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanConciergeUpdateRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_CONCIERGE_UPDATE_RES;

        public S2CClanClanConciergeUpdateRes()
        {
            CP = 0;
        }

        public C2SClanClanConciergeUpdateReq ConciergeUpdate { get; set; }
        public uint CP { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanConciergeUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanConciergeUpdateRes obj)
            {
                C2SClanClanConciergeUpdateReq req = obj.ConciergeUpdate;
                WriteUInt64(buffer, 0);
                WriteUInt32(buffer, req.ConciergeId);
                uint i = obj.CP-req.RequireCP;
                WriteUInt32(buffer, i);
            }

            public override S2CClanClanConciergeUpdateRes Read(IBuffer buffer)
            {
                S2CClanClanConciergeUpdateRes obj = new S2CClanClanConciergeUpdateRes();
                return obj;
            }
        }

    }
}
