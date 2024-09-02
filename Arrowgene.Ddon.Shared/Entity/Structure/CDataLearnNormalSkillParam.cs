using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLearnNormalSkillParam
    {
        public CDataLearnNormalSkillParam(CDataNormalSkillParam skill)
        {
            SkillIndex = skill.Index;
        }

        public CDataLearnNormalSkillParam()
        {
            SkillIndex=0;
        }

        public uint SkillIndex { get; set; }

        public class Serializer : EntitySerializer<CDataLearnNormalSkillParam>
        {
            public override void Write(IBuffer buffer, CDataLearnNormalSkillParam obj)
            {
                WriteUInt32(buffer, obj.SkillIndex);
            }

            public override CDataLearnNormalSkillParam Read(IBuffer buffer)
            {
                CDataLearnNormalSkillParam obj = new CDataLearnNormalSkillParam();
                obj.SkillIndex = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
