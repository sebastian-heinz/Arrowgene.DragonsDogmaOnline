using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataNormalSkillParam
    {
        public CDataNormalSkillParam()
        {
            Job=0;
            Index=0;
            SkillNo=0;
            PreSkillNo=0;
        }

        public JobId Job { get; set; }
        public uint Index { get; set; }
        public uint SkillNo { get; set; }
        public uint PreSkillNo { get; set; }

        public class Serializer : EntitySerializer<CDataNormalSkillParam>
        {
            public override void Write(IBuffer buffer, CDataNormalSkillParam obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.Index);
                WriteUInt32(buffer, obj.SkillNo);
                WriteUInt32(buffer, obj.PreSkillNo);
            }

            public override CDataNormalSkillParam Read(IBuffer buffer)
            {
                CDataNormalSkillParam obj = new CDataNormalSkillParam();
                obj.Job = (JobId) ReadByte(buffer);
                obj.Index = ReadUInt32(buffer);
                obj.SkillNo = ReadUInt32(buffer);
                obj.PreSkillNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
