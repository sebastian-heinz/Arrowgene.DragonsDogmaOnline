using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class CustomSkill
    {
        public JobId Job { get; set; }
        public byte SlotNo { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public CDataSetAcquirementParam AsCDataSetAcquirementParam()
        {
            return new CDataSetAcquirementParam()
            {
                Job = this.Job,
                SlotNo = this.SlotNo,
                AcquirementNo = this.SkillId,
                AcquirementLv = this.SkillLv
            };
        }

        public CDataContextAcquirementData AsCDataContextAcquirementData()
        {
            return new CDataContextAcquirementData()
            {
                SlotNo = this.SlotNo,
                AcquirementNo = this.SkillId,
                AcquirementLv = this.SkillLv
            };
        }
    }
}