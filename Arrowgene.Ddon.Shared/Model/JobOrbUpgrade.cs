using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class JobOrbUpgrade
    {
        public JobOrbUpgrade()
        {
            QuestDependencies = new List<CDataCommonU32>();
            UnlockDependencies = new List<CDataCommonU32>();
            SpecialConditionDependencies = new List<CDataCommonU32>();
        }

        public uint ElementId { get; private set; }
        public uint Amount { get; private set; }
        public OrbGainParamType GainType { get; private set; }
        public CustomSkillId CustomSkillId { get; private set; }
        public AbilityId AbilityId { get; private set; }
        public uint PosX { get; private set; }
        public uint PosY { get; private set; }
        public uint Cost { get; private set; }
        public JobId JobId { get; private set; }
        public OrbTreeType OrbTreeType { get; private set; }
        public WalletType WalletType { get; private set; }

        public List<CDataCommonU32> QuestDependencies { get; private set; }
        public List<CDataCommonU32> UnlockDependencies { get; private set; }
        public List<CDataCommonU32> SpecialConditionDependencies { get; private set; }

        public bool IsCustomSkill()
        {
            return GainType == OrbGainParamType.JobCustomSkill;
        }

        public bool IsAbility()
        {
            return GainType == OrbGainParamType.JobAbility;
        }

        public static JobId GetJobIdFromReleaseId(uint releaseId)
        {
            return (JobId)((releaseId >> 28) & 0xf);
        }

        public static OrbTreeType GetOrbTreeTypeFromReleaseId(uint releaseId)
        {
            return (OrbTreeType)((releaseId >> 24) & 0xf);
        }

        private uint JobUniqueId(uint elementId, OrbTreeType orbTreeType, JobId jobId)
        {
            return elementId | ((uint)jobId << 28) | ((uint)orbTreeType << 24);
        }

        public JobOrbUpgrade Id(uint elementId, OrbTreeType orbTreeType, JobId jobId)
        {
            this.ElementId = JobUniqueId(elementId, orbTreeType, jobId);
            this.JobId = jobId;
            this.OrbTreeType = orbTreeType;
            return this;
        }

        public JobOrbUpgrade BloodOrbCost(uint amount)
        {
            this.Cost = amount;
            this.WalletType = WalletType.BloodOrbs;
            return this;
        }

        public JobOrbUpgrade HighOrbCost(uint amount)
        {
            this.Cost = amount;
            this.WalletType = WalletType.HighOrbs;
            return this;
        }

        public JobOrbUpgrade Location(uint x, uint y)
        {
            this.PosX = x;
            this.PosY = y;
            return this;
        }

        public JobOrbUpgrade Unlocks(OrbGainParamType gainType, uint amount)
        {
            this.GainType = gainType;
            this.Amount = amount;
            return this;
        }

        public JobOrbUpgrade Unlocks(CustomSkillId customSkill)
        {
            this.GainType = OrbGainParamType.JobCustomSkill;
            this.CustomSkillId = customSkill;
            return this;
        }

        public JobOrbUpgrade Unlocks(AbilityId ability)
        {
            this.GainType = OrbGainParamType.JobAbility;
            this.AbilityId = ability;
            return this;
        }

        public JobOrbUpgrade HasUnlockDependency(uint elementId)
        {
            this.UnlockDependencies.Add(new CDataCommonU32() { Value = JobUniqueId(elementId, OrbTreeType, JobId) });
            return this;
        }

        public JobOrbUpgrade HasUnlockDependencies(List<uint> elementIds)
        {
            foreach (var elementId in elementIds)
            {
                HasUnlockDependency(elementId);
            }
            return this;
        }

        public JobOrbUpgrade HasUnlockDependencies(params uint[] elementIds)
        {
            foreach (var elementId in elementIds)
            {
                HasUnlockDependency(elementId);
            }
            return this;
        }

        public JobOrbUpgrade HasQuestDependency(QuestId questId)
        {
            return HasQuestDependency((uint)questId);
        }

        public JobOrbUpgrade HasQuestDependency(uint questId)
        {
            this.QuestDependencies.Add(new CDataCommonU32() { Value = questId });
            return this;
        }

        public JobOrbUpgrade HasQuestDependencies(List<uint> questIds)
        {
            foreach (var questId in questIds)
            {
                HasQuestDependency(questId);
            }
            return this;
        }

        public JobOrbUpgrade HasQuestDependencies(List<QuestId> questIds)
        {
            foreach (var questId in questIds)
            {
                HasQuestDependency(questId);
            }
            return this;
        }

        public JobOrbUpgrade HasQuestDependencies(params uint[] questIds)
        {
            foreach (var questId in questIds)
            {
                HasQuestDependency(questId);
            }
            return this;
        }

        public JobOrbUpgrade HasSpecialConditionDependency(uint specialConditionId)
        {
            this.SpecialConditionDependencies.Add(new CDataCommonU32() { Value = specialConditionId });
            return this;
        }

        public JobOrbUpgrade HasSpecialConditionDependencies(params uint[] specialConditionIds)
        {
            foreach (var specialConditionId in specialConditionIds)
            {
                HasSpecialConditionDependency(specialConditionId);
            }
            return this;
        }

        public CDataJobOrbDevoteElement ToCDataJobOrbDevoteElement(bool isReleased)
        {
            var result = new CDataJobOrbDevoteElement()
            {
                ElementId = this.ElementId,
                JobId = this.JobId,
                RequireOrb = this.Cost,
                OrbParamType = this.GainType,
                PosX = this.PosX,
                PosY = this.PosY,
                IsReleased = isReleased,
                RequiredElementIDList = this.UnlockDependencies,
                RequiredQuestList = this.QuestDependencies,
                WalletType = this.WalletType,
                SpecialConditionIdList = this.SpecialConditionDependencies,
            };

            if (IsCustomSkill())
            {
                result.ParamId = this.CustomSkillId.ReleaseId();
                result.ParamValue = 0;
            }
            else if (IsAbility())
            {
                result.ParamId = (uint)this.AbilityId;
                result.ParamValue = 0;
            }
            else
            {
                result.ParamId = 0;
                result.ParamValue = this.Amount;
            }

            return result;
        }
    }
}
