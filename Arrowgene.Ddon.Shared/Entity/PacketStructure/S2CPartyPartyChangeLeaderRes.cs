using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyChangeLeaderRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_CHANGE_LEADER_RES;

        public class Serializer : PacketEntitySerializer<S2CPartyPartyChangeLeaderRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyChangeLeaderRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPartyPartyChangeLeaderRes Read(IBuffer buffer)
            {
                S2CPartyPartyChangeLeaderRes obj = new S2CPartyPartyChangeLeaderRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
