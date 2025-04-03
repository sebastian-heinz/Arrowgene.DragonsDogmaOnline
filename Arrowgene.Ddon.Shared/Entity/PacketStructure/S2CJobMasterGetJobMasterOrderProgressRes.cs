using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobMasterGetJobMasterOrderProgressRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_RES;

        public S2CJobMasterGetJobMasterOrderProgressRes()
        {
            ActiveJobOrderList = new List<CDataActiveJobOrder>();
        }

        public JobId JobId { get; set; }
        public List<CDataActiveJobOrder> ActiveJobOrderList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobMasterGetJobMasterOrderProgressRes>
        {
            public override void Write(IBuffer buffer, S2CJobMasterGetJobMasterOrderProgressRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.JobId);
                WriteEntityList(buffer, obj.ActiveJobOrderList);
            }

            public override S2CJobMasterGetJobMasterOrderProgressRes Read(IBuffer buffer)
            {
                S2CJobMasterGetJobMasterOrderProgressRes obj = new S2CJobMasterGetJobMasterOrderProgressRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId) ReadByte(buffer);
                obj.ActiveJobOrderList = ReadEntityList<CDataActiveJobOrder>(buffer);
                return obj;
            }
        }
    }
}
