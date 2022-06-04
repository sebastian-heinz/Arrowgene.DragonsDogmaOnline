using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobChangeJobReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_CHANGE_JOB_REQ;

        public C2SJobChangeJobReq()
        {
            JobId=0;
        }

        public JobId JobId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobChangeJobReq>
        {

            public override void Write(IBuffer buffer, C2SJobChangeJobReq obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
            }

            public override C2SJobChangeJobReq Read(IBuffer buffer)
            {
                C2SJobChangeJobReq obj = new C2SJobChangeJobReq();
                obj.JobId = (JobId) ReadByte(buffer);
                return obj;
            }
        }
    }
}
