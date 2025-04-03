using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobMasterReportJobOrderProgressRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_RES;

        public S2CJobMasterReportJobOrderProgressRes()
        {
            ReleaseElementList = new List<CDataReleaseElement>();
            NewOrderIdList = new List<CDataCommonU32>();
        }

        public JobId JobId { get; set; }
        public List<CDataReleaseElement> ReleaseElementList { get; set; }
        public List<CDataCommonU32> NewOrderIdList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobMasterReportJobOrderProgressRes>
        {
            public override void Write(IBuffer buffer, S2CJobMasterReportJobOrderProgressRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.JobId);
                WriteEntityList(buffer, obj.ReleaseElementList);
                WriteEntityList(buffer, obj.NewOrderIdList);
            }

            public override S2CJobMasterReportJobOrderProgressRes Read(IBuffer buffer)
            {
                S2CJobMasterReportJobOrderProgressRes obj = new S2CJobMasterReportJobOrderProgressRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.ReleaseElementList = ReadEntityList<CDataReleaseElement>(buffer);
                obj.NewOrderIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}

