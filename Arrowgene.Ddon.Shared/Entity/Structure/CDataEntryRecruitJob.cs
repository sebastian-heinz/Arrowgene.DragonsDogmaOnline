using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryRecruitJob
    {
        public CDataEntryRecruitJob()
        {
        }

        public JobId Job { get; set; }

        public class Serializer : EntitySerializer<CDataEntryRecruitJob>
        {
            public override void Write(IBuffer buffer, CDataEntryRecruitJob obj)
            {
                WriteByte(buffer, (byte) obj.Job);
            }

            public override CDataEntryRecruitJob Read(IBuffer buffer)
            {
                CDataEntryRecruitJob obj = new CDataEntryRecruitJob();
                obj.Job = (JobId) ReadByte(buffer);
                return obj;
            }
        }
    }
}
