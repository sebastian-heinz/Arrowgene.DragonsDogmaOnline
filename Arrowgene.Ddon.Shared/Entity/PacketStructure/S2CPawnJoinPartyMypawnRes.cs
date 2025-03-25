using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnJoinPartyMyPawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_JOIN_PARTY_MYPAWN_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnJoinPartyMyPawnRes>
        {

            public override void Write(IBuffer buffer, S2CPawnJoinPartyMyPawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnJoinPartyMyPawnRes Read(IBuffer buffer)
            {
                S2CPawnJoinPartyMyPawnRes obj = new S2CPawnJoinPartyMyPawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }

    }

}
