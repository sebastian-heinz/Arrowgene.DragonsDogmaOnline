using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobMasterGetJobMasterOrderProgressReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_REQ;

        public C2SJobMasterGetJobMasterOrderProgressReq()
        {
        }

        public JobId JobId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobMasterGetJobMasterOrderProgressReq>
        {

            public override void Write(IBuffer buffer, C2SJobMasterGetJobMasterOrderProgressReq obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
            }

            public override C2SJobMasterGetJobMasterOrderProgressReq Read(IBuffer buffer)
            {
                C2SJobMasterGetJobMasterOrderProgressReq obj = new C2SJobMasterGetJobMasterOrderProgressReq();
                obj.JobId = (JobId)ReadByte(buffer);
                return obj;
            }
        }
    }
}
