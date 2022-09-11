using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnJoinPartyMypawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_JOIN_PARTY_MYPAWN_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnJoinPartyMypawnRes>
        {

            public override void Write(IBuffer buffer, S2CPawnJoinPartyMypawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnJoinPartyMypawnRes Read(IBuffer buffer)
            {
                S2CPawnJoinPartyMypawnRes obj = new S2CPawnJoinPartyMypawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }

    }

}
