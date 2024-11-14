using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnExpeditionGetSallyInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnExpeditionGetSallyInfoReq>
        {
            public override void Write(IBuffer buffer, C2SPawnExpeditionGetSallyInfoReq obj)
            {
            }

            public override C2SPawnExpeditionGetSallyInfoReq Read(IBuffer buffer)
            {
                C2SPawnExpeditionGetSallyInfoReq obj = new C2SPawnExpeditionGetSallyInfoReq();
                return obj;
            }
        }
    }
}
