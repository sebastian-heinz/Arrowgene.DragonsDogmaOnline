using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class CustomSkill
    {
        public JobId Job { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public CDataSetAcquirementParam AsCDataSetAcquirementParam(byte slotNo)
        {
            return new CDataSetAcquirementParam()
            {
                Job = this.Job,
                SlotNo = slotNo,
                AcquirementNo = this.SkillId,
                AcquirementLv = this.SkillLv
            };
        }

        public CDataContextAcquirementData AsCDataContextAcquirementData(byte slotNo)
        {
            return new CDataContextAcquirementData()
            {
                SlotNo = slotNo,
                AcquirementNo = this.SkillId,
                AcquirementLv = this.SkillLv
            };
        }

        public CDataLearnedSetAcquirementParam AsCDataLearnedSetAcquirementParam()
        {
            return new CDataLearnedSetAcquirementParam()
            {
                Job = this.Job,
                Type = 0,
                AcquirementNo = this.SkillId,
                AcquirementLv = this.SkillLv,
                AcquirementParamId = 0
            };
        }
    }
}
