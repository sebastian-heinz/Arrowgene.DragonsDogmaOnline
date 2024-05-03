using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextJobData
    {
        public CDataContextJobData(CDataCharacterJobData characterJobData)
        {
            Job = characterJobData.Job;
            Lv = (ushort) characterJobData.Lv;
            Exp = characterJobData.Exp;
            JobPoint = characterJobData.JobPoint;
        }

        public CDataContextJobData()
        {
            Job=0;
            Lv=0;
            Exp=0;
            JobPoint=0;
        }

        public static List<CDataContextJobData> FromCDataCharacterJobData(List<CDataCharacterJobData> characterJobData)
        {
            List<CDataContextJobData> obj = new List<CDataContextJobData>();
            foreach (var item in characterJobData)
            {
                obj.Add(new CDataContextJobData(item));
            }
            return obj;
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
