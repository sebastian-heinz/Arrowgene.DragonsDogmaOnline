using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class ISkillAugmentation
    {
        public abstract JobId JobId { get; }
        public abstract OrbTreeType OrbTreeType { get; }
        public List<JobOrbUpgrade> Upgrades { get; }

        public ISkillAugmentation()
        {
            Upgrades = new List<JobOrbUpgrade>();
        }

        public JobOrbUpgrade AddNode(uint elementId)
        {
            JobOrbUpgrade upgrade = new JobOrbUpgrade()
                .Id(elementId, OrbTreeType, JobId);
            Upgrades.Add(upgrade);

            return upgrade;
        }
    }
}
