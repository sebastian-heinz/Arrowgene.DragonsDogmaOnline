using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobMasterReportJobOrderProgressReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_REQ;

        public C2SJobMasterReportJobOrderProgressReq()
        {
        }

        public JobId JobId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobMasterReportJobOrderProgressReq>
        {

            public override void Write(IBuffer buffer, C2SJobMasterReportJobOrderProgressReq obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
            }

            public override C2SJobMasterReportJobOrderProgressReq Read(IBuffer buffer)
            {
                C2SJobMasterReportJobOrderProgressReq obj = new C2SJobMasterReportJobOrderProgressReq();
                obj.JobId = (JobId)ReadByte(buffer);
                return obj;
            }
        }
    }
}
