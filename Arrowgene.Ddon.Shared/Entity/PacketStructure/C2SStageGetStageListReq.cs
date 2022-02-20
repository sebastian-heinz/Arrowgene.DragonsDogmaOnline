using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageGetStageListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_GET_STAGE_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SStageGetStageListReq>
        {

            public override void Write(IBuffer buffer, C2SStageGetStageListReq obj)
            {
            }

            public override C2SStageGetStageListReq Read(IBuffer buffer)
            {
                return new C2SStageGetStageListReq();
            }
        }
    }
}
