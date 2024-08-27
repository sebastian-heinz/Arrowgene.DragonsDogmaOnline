using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnDeleteMyPawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_DELETE_MYPAWN_RES;

        public S2CPawnDeleteMyPawnRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CPawnDeleteMyPawnRes>
        {
            public override void Write(IBuffer buffer, S2CPawnDeleteMyPawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnDeleteMyPawnRes Read(IBuffer buffer)
            {
                S2CPawnDeleteMyPawnRes obj = new S2CPawnDeleteMyPawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
