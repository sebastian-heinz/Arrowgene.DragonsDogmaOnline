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
        }

        public uint ElementId { get; private set; }
        public uint Amount { get; private set; }
        public OrbGainParamType GainType { get; private set; }
        public CustomSkillId CustomSkill { get; private set; }
        public AbilityId Augment { get; private set; }
        public uint PosX { get; private set; }
        public uint PosY { get; private set; }
        public uint Cost { get; private set; }
        public JobId JobId { get; private set; }

        public List<CDataCommonU32> QuestDependencies { get; private set; }
        public List<CDataCommonU32> UnlockDependencies { get; private set; }

        public bool IsCustomSkill()
        {
            return GainType == OrbGainParamType.JobCustomSkill;
        }

        public bool IsAugment()
        {
            return GainType == OrbGainParamType.JobAugment;
        }

        private uint JobUniqueId(uint elementId, JobId jobId)
        {
            return elementId | ((uint)jobId << 28);
        }

        public JobOrbUpgrade Id(uint elementId, JobId jobId)
        {
            this.ElementId = JobUniqueId(elementId, jobId);
            this.JobId = jobId;
            return this;
        }

        public JobOrbUpgrade BloodOrbCost(uint amount)
        {
            this.Cost = amount;
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
            this.CustomSkill = customSkill;
            return this;
        }

        public JobOrbUpgrade Unlocks(AbilityId augment)
        {
            this.GainType = OrbGainParamType.JobAugment;
            this.Augment = augment;
            return this;
        }

        public JobOrbUpgrade HasUnlockDependency(uint elementId)
        {
            this.UnlockDependencies.Add(new CDataCommonU32() { Value = JobUniqueId(elementId, JobId) });
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

        public CDataJobOrbDevoteElement ToCDataJobOrbDevoteElement()
        {
            var result = new CDataJobOrbDevoteElement()
            {
                ElementId = this.ElementId,
                JobId = this.JobId,
                RequireOrb = this.Cost,
                OrbParamType = this.GainType,
                PosX = this.PosX,
                PosY = this.PosY,
                IsReleased = false, // TODO: Query from DB
                RequiredElementIDList = this.UnlockDependencies,
                RequiredQuestList = this.QuestDependencies,
            };

            if (IsCustomSkill())
            {
                result.ParamId = this.CustomSkill.GetCustomSkillRelativeMapping();
                result.ParamValue = 0;
            }
            else if (IsAugment())
            {
                result.ParamId = (uint)this.Augment;
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
