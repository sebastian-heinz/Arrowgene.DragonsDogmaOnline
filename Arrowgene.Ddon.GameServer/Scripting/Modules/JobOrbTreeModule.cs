using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class JobOrbTreeModule : GameServerScriptModule
    {
        private string _ModuleRoot;
        public override string ModuleRoot
        {
            get { return _ModuleRoot; }
        }

        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public Dictionary<JobId, List<JobOrbUpgrade>> SkillAugmentations { get; private set; }

        public JobOrbTreeModule(string moduleRoot)
        {
            _ModuleRoot = moduleRoot;
            SkillAugmentations = new Dictionary<JobId, List<JobOrbUpgrade>>();
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            var skillAugmentationInformation = (ISkillAugmentation) result.ReturnValue;
            if (skillAugmentationInformation != null)
            {
                SkillAugmentations[skillAugmentationInformation.JobId] = skillAugmentationInformation.Upgrades;
            }

            return true;
        }
    }
}
