using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextJobData
    {
        public CDataContextJobData()
        {
            Job=0;
            Lv=0;
            Exp=0;
            JobPoint=0;
        }

        public JobId Job { get; set; }
        public ushort Lv { get; set; }
        public ulong Exp { get; set; }
        public ulong JobPoint { get; set; }

        public class Serializer : EntitySerializer<CDataContextJobData>
        {
            public override void Write(IBuffer buffer, CDataContextJobData obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt16(buffer, obj.Lv);
                WriteUInt64(buffer, obj.Exp);
                WriteUInt64(buffer, obj.JobPoint);
            }

            public override CDataContextJobData Read(IBuffer buffer)
            {
                CDataContextJobData obj = new CDataContextJobData();
                obj.Job = (JobId) ReadByte(buffer);
                obj.Lv = ReadUInt16(buffer);
                obj.Exp = ReadUInt64(buffer);
                obj.JobPoint = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}