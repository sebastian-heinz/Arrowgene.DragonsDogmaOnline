using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class LearnedNormalSkillsAsset
    {
        public LearnedNormalSkillsAsset()
        {
            LearnedNormalSkills = new Dictionary<JobId, List<LearnedNormalSkill>>();
        }

        public Dictionary<JobId, List<LearnedNormalSkill>> LearnedNormalSkills { get; set; }
    }
}
