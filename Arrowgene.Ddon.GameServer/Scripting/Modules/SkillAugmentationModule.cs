using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server.Scripting;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class SkillAugmentationModule : GameServerScriptModule
    {
        public override string ModuleRoot => "skill_augmentation";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public Dictionary<JobId, List<JobOrbUpgrade>> SkillAugmentations { get; private set; }

        public SkillAugmentationModule()
        {
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
