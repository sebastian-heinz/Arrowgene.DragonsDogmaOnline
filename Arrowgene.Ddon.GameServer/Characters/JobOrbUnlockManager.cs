using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobOrbUnlockManager
    {
        private DdonGameServer Server;

        public JobOrbUnlockManager(DdonGameServer server)
        {
            Server = server;
        }

        public List<CDataJobOrbDevoteElement> GetUpgradeList(GameClient client, JobId jobId)
        {
            var jobOrbUpgrades = Server.ScriptManager.SkillAugmentationModule.SkillAugmentations;
            if (!jobOrbUpgrades.ContainsKey(jobId))
            {
                return new List<CDataJobOrbDevoteElement>();
            }

            return jobOrbUpgrades[jobId].Select(x => x.ToCDataJobOrbDevoteElement(client.Character.ReleasedExtendedJobParams[jobId].Contains(x.ElementId))).ToList();
        }

        public Dictionary<JobId, HashSet<uint>> GetReleasedElements(Character character, DbConnection? connectionIn = null)
        {
            var results = new Dictionary<JobId, HashSet<uint>>();
            foreach (var jobId in (JobId[])JobId.GetValues(typeof(JobId)))
            {
                if (jobId == JobId.None)
                {
                    continue;
                }
                results[jobId] = Server.Database.GetSkillAugmentationReleasedElements(character.CharacterId, jobId, connectionIn);
            }
            return results;
        }

        private void UpdateExtendedParams(Dictionary<JobId, CDataOrbGainExtendParam> extendedParams, JobId jobId, JobOrbUpgrade upgrade)
        {
            CDataOrbGainExtendParam extendParams = extendedParams[JobId.None];
            if (upgrade.GainType.IsJobOnlyParam())
            {
                extendParams = extendedParams[jobId];
            }

            switch (upgrade.GainType)
            {
                case OrbGainParamType.JobHpMax:
                case OrbGainParamType.AllJobsHpMax:
                    extendParams.HpMax += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.JobStaminaMax:
                case OrbGainParamType.AllJobsStaminaMax:
                    extendParams.StaminaMax += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.JobPhysicalAttack:
                case OrbGainParamType.AllJobsPhysicalAttack:
                    extendParams.Attack += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.JobPhysicalDefence:
                case OrbGainParamType.AllJobsPhysicalDefence:
                    extendParams.Defence += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.JobMagicalAttack:
                case OrbGainParamType.AllJobsMagicalAttack:
                    extendParams.MagicAttack += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.JobMagicalDefence:
                case OrbGainParamType.AllJobsMagicalDefence:
                    extendParams.MagicDefence += (ushort)upgrade.Amount;
                    break;
            }
        }

        public void EvaluateJobOrbTreeUnlocks(Character character)
        {
            var jobOrbUpgrades = Server.ScriptManager.SkillAugmentationModule.SkillAugmentations;

            character.ExtendedJobParams[JobId.None] = new CDataOrbGainExtendParam();
            foreach (var jobId in jobOrbUpgrades.Keys)
            {
                character.ExtendedJobParams[jobId] = new CDataOrbGainExtendParam();
                character.UnlockedCustomSkills[jobId] = new HashSet<uint>();
                character.UnlockedAbilities[jobId] = new HashSet<uint>();
                foreach (var element in jobOrbUpgrades[jobId])
                {
                    if (character.ReleasedExtendedJobParams[jobId].Contains(element.ElementId))
                    {
                        if (element.IsAbility())
                        {
                            character.UnlockedAbilities[jobId].Add((uint) element.AbilityId);
                        }
                        else if (element.IsCustomSkill())
                        {
                            character.UnlockedCustomSkills[jobId].Add(element.CustomSkillId.ReleaseId());
                        }
                        else
                        {
                            UpdateExtendedParams(character.ExtendedJobParams, jobId, element);
                        }
                    }
                }
            }
        }

        public float CalculatePercentCompleted(Character character, JobId jobId)
        {
            var jobOrbUpgrades = Server.ScriptManager.SkillAugmentationModule.SkillAugmentations;
            if (!jobOrbUpgrades.ContainsKey(jobId))
            {
                return 0;
            }
            return (float)character.ReleasedExtendedJobParams[jobId].Count / (float)jobOrbUpgrades[jobId].Count;
        }

        public List<CDataJobOrbTreeStatus> GetJobOrbTreeStatus(Character character)
        {
            var results = new List<CDataJobOrbTreeStatus>();
            foreach (var jobId in character.ReleasedExtendedJobParams.Keys)
            {
                results.Add(new CDataJobOrbTreeStatus()
                {
                    JobId = jobId,
                    IsReleased = character.HasContentReleased(jobId.SkillAugmentationReleaseId()),
                    Rate = Server.JobOrbUnlockManager.CalculatePercentCompleted(character, jobId)
                });
            }
            return results;
        }

        public PacketQueue ReleaseElement(GameClient client, JobId jobId, uint elementId, DbConnection? connectionIn = null)
        {
            var packets = new PacketQueue();

            var jobOrbUpgrades = Server.ScriptManager.SkillAugmentationModule.SkillAugmentations;

            var upgrade = jobOrbUpgrades[jobId].Where(x => x.ElementId == elementId).FirstOrDefault() ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CONTENTS_RELEASE_NOT_JOB_ORB_TREE, $"An element with the ID {elementId} doesn't exist for {jobId}");

            var amount = Server.WalletManager.GetWalletAmount(client.Character, WalletType.BloodOrbs);
            if (amount < upgrade.Cost)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_TREE_RELEASE_COST_LACK);
            }

            packets.Enqueue(client, Server.WalletManager.RemoveFromWalletNtc2(client.Character, WalletType.BloodOrbs, upgrade.Cost, connectionIn));

            Server.Database.InsertSkillAugmentationReleasedElement(client.Character.CharacterId, jobId, elementId, connectionIn);
            client.Character.ReleasedExtendedJobParams[jobId].Add(elementId);

            if (upgrade.IsAbility())
            {
                Server.CharacterManager.UnlockAbility(client.Character, jobId, (uint)upgrade.AbilityId, 1);

                // Handle players who had existing abilities blocked by addition of S2 tree
                var existing = client.Character.LearnedAbilities.Where(x => x.AbilityId == (uint) upgrade.AbilityId).FirstOrDefault();
                if (existing == null)
                {
                    Server.JobManager.UnlockAbility(client, client.Character, jobId, (uint)upgrade.AbilityId, 1, connectionIn);
                }

                packets.Enqueue(client, new S2CSkillAcquirementLearnNtc()
                {
                    AbilityParamList = new List<CDataAbilityLevelBaseParam>()
                    {
                       new CDataAbilityLevelBaseParam()
                       {
                           AbilityNo = (uint) upgrade.AbilityId,
                           AbilityLv = existing?.AbilityLv ?? 1,
                       }
                    }
                });
            }
            else if (upgrade.IsCustomSkill())
            {
                Server.CharacterManager.UnlockCustomSkill(client.Character, jobId, upgrade.CustomSkillId.ReleaseId(), 1);

                // Handle players who had existing skills blocked by addition of S2 tree
                var existing = client.Character.LearnedCustomSkills.Where(x => x.SkillId == upgrade.CustomSkillId.ReleaseId() && x.Job == jobId).FirstOrDefault();
                if (existing == null)
                {
                    Server.JobManager.UnlockCustomSkill(client, client.Character, jobId, upgrade.CustomSkillId.ReleaseId(), 1, connectionIn);
                }

                packets.Enqueue(client, new S2CSkillAcquirementLearnNtc()
                {
                    SkillParamList = new List<CDataSkillLevelBaseParam>()
                    {
                       new CDataSkillLevelBaseParam()
                       {
                           Job = jobId,
                           SkillNo = upgrade.CustomSkillId.ReleaseId(),
                           SkillLv = existing?.SkillLv ?? 1,
                       }
                    }
                });
            }
            UpdateExtendedParams(client.Character.ExtendedJobParams, jobId, upgrade);

            return packets;
        }
    }
}
