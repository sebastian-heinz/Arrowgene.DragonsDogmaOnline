using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Ability
    {
        public JobId EquippedToJob { get; set; }
        public JobId Job { get; set; }
        public byte SlotNo { get; set; } // Slots start at 1
        public uint AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public CDataSetAcquirementParam AsCDataSetAcquirementParam()
        {
            return new CDataSetAcquirementParam()
            {
                Job = this.Job,
                SlotNo = this.SlotNo,
                AcquirementNo = this.AbilityId,
                AcquirementLv = this.AbilityLv
            };
        }

        public CDataContextAcquirementData AsCDataContextAcquirementData()
        {
            return new CDataContextAcquirementData()
            {
                SlotNo = this.SlotNo,
                AcquirementNo = this.AbilityId,
                AcquirementLv = this.AbilityLv
            };
        }
    }
}