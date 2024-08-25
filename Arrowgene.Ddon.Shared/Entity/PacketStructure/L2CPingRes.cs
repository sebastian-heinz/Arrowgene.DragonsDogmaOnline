using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CPingRes : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_PING_RES;

        public L2CPingRes()
        {            
        }


        public class Serializer : PacketEntitySerializer<L2CPingRes>
        {
            public override void Write(IBuffer buffer, L2CPingRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override L2CPingRes Read(IBuffer buffer)
            {
                L2CPingRes obj = new L2CPingRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }

    }
}