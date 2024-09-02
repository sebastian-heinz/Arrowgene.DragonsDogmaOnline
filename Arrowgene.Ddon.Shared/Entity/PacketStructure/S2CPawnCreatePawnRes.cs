using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnCreatePawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_CREATE_MYPAWN_RES;
        public S2CPawnCreatePawnRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CPawnCreatePawnRes>
        {

            public override void Write(IBuffer buffer, S2CPawnCreatePawnRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnCreatePawnRes Read(IBuffer buffer)
            {
                S2CPawnCreatePawnRes obj = new S2CPawnCreatePawnRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
