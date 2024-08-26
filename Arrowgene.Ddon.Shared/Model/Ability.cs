using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Ability
    {
        public JobId Job { get; set; }
        public uint AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public CDataSetAcquirementParam AsCDataSetAcquirementParam(byte slotNo)
        {
            return new CDataSetAcquirementParam()
            {
                Job = this.Job,
                SlotNo = slotNo,
                AcquirementNo = this.AbilityId,
                AcquirementLv = this.AbilityLv
            };
        }

        public CDataContextAcquirementData AsCDataContextAcquirementData(byte slotNo)
        {
            return new CDataContextAcquirementData()
            {
                SlotNo = slotNo,
                AcquirementNo = this.AbilityId,
                AcquirementLv = this.AbilityLv
            };
        }

        public CDataLearnedSetAcquirementParam AsCDataLearnedSetAcquirementParam()
        {
            return new CDataLearnedSetAcquirementParam()
            {
                Job = this.Job,
                Type = 0,
                AcquirementNo = this.AbilityId,
                AcquirementLv = this.AbilityLv,
                AcquirementParamId = 0
            };
        }
    }
}
