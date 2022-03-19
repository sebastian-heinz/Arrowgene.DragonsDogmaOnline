using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobGetJobChangeListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_GET_JOB_CHANGE_LIST_REQ;

        public C2SJobGetJobChangeListReq()
        {
        }
        
        public class Serializer : PacketEntitySerializer<C2SJobGetJobChangeListReq>
        {

            public override void Write(IBuffer buffer, C2SJobGetJobChangeListReq obj)
            {
            }

            public override C2SJobGetJobChangeListReq Read(IBuffer buffer)
            {
                return new C2SJobGetJobChangeListReq();
            }
        }
    }
}
