using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLearnNormalSkillParam
    {
        public CDataLearnNormalSkillParam(CDataNormalSkillParam skill)
        {
            SkillNo = skill.SkillNo;
        }

        public CDataLearnNormalSkillParam()
        {
            SkillNo=0;
        }

        public uint SkillNo { get; set; }

        public class Serializer : EntitySerializer<CDataLearnNormalSkillParam>
        {
            public override void Write(IBuffer buffer, CDataLearnNormalSkillParam obj)
            {
                WriteUInt32(buffer, obj.SkillNo);
            }

            public override CDataLearnNormalSkillParam Read(IBuffer buffer)
            {
                CDataLearnNormalSkillParam obj = new CDataLearnNormalSkillParam();
                obj.SkillNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
