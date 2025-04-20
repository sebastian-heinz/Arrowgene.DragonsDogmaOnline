using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Linq;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class JobMasterAssetDeserializer : IAssetDeserializer<JobMasterAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(JobMasterAssetDeserializer));

        public JobMasterAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            JobMasterAsset asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var jJobMasterData in document.RootElement.EnumerateArray())
            {
                if (!Enum.TryParse(jJobMasterData.GetProperty("job").GetString(), true, out JobId jobId))
                {
                    var name = jJobMasterData.GetProperty("job_id").GetString();
                    Logger.Error($"Failed to parse JobId={name}. Skipping.");
                    continue;
                }

                if (!asset.JobOrders.ContainsKey(jobId))
                {
                    asset.JobOrders[jobId] = new Dictionary<ReleaseType, Dictionary<uint, List<CDataActiveJobOrder>>>();
                    asset.JobOrders[jobId][ReleaseType.CustomSkill] = new Dictionary<uint, List<CDataActiveJobOrder>>();
                    asset.JobOrders[jobId][ReleaseType.Augment] = new Dictionary<uint, List<CDataActiveJobOrder>>();
                }

                if (!Enum.TryParse(jJobMasterData.GetProperty("release_type").GetString(), true, out ReleaseType releaseType))
                {
                    var name = jJobMasterData.GetProperty("name").GetString();
                    Logger.Error($"Failed to parse JobTrainingReleaseType={name}. Skipping.");
                    continue;
                }

                foreach (var jSkill in jJobMasterData.GetProperty("skills").EnumerateArray())
                {
                    uint releaseId;
                    if (releaseType == ReleaseType.CustomSkill)
                    {
                        if (!Enum.TryParse(jSkill.GetProperty("name").GetString(), true, out CustomSkillId customSkillId))
                        {
                            var name = jSkill.GetProperty("name").GetString();
                            Logger.Error($"Failed to parse JobTrainingReleaseType={name}. Skipping.");
                            continue;
                        }

                        releaseId = customSkillId.ReleaseId();
                    }
                    else if (releaseType == ReleaseType.Augment)
                    {
                        releaseId = jSkill.GetProperty("id").GetUInt32();
                    }
                    else
                    {
                        throw new Exception("Invalid upgrade type!");
                    }

                    asset.JobOrders[jobId][releaseType][releaseId] = new List<CDataActiveJobOrder>();

                    var tasks = new Dictionary<uint, CDataActiveJobOrder>();
                    foreach (var jTask in jSkill.GetProperty("tasks").EnumerateArray())
                    {
                        var releaseLv = jTask.GetProperty("skill_level").GetByte();
                        if (!tasks.ContainsKey(releaseLv))
                        {
                            tasks[releaseLv] = new CDataActiveJobOrder()
                            {
                                ReleaseType = releaseType,
                                ReleaseId = releaseId,
                                ReleaseLv = releaseLv,
                            };
                        }

                        if (!Enum.TryParse(jTask.GetProperty("objective").GetString(), true, out JobOrderCondType conditionType))
                        {
                            var name = jTask.GetProperty("objective").GetString();
                            Logger.Error($"Failed to parse Objectvie={name}. Skipping.");
                            continue;
                        }

                        var condition = new CDataJobOrderProgress()
                        {
                            ConditionType = conditionType
                        };

                        if (conditionType == JobOrderCondType.Hunt)
                        {
                            var enemyId = EnemyId.None;

                            var enemyType = jTask.GetProperty("enemy_type").GetString();
                            if (enemyType == "OrbEnemy")
                            {
                                condition.ConditionType = JobOrderCondType.BloodOrbEnemies;
                            }
                            else
                            {
                                if (!Enum.TryParse(enemyType, true, out enemyId))
                                {
                                    if (!Enum.TryParse($"{enemyType}0", true, out enemyId))
                                    {
                                        if (!UniqueNameReplacements.ContainsKey(enemyType))
                                        {
                                            Logger.Error($"Failed to parse EnemyId={enemyType}. Skipping.");
                                            continue;
                                        }
                                        enemyId = UniqueNameReplacements[enemyType];
                                    }
                                }
                            }

                            condition.TargetId = Enemy.GetNameId(enemyId);
                            if (condition.TargetId == 0 && condition.ConditionType != JobOrderCondType.BloodOrbEnemies)
                            {
                                Logger.Error($"Something went wrong with {jobId}, {releaseId}");
                            }

                            condition.TargetRank = jTask.GetProperty("enemy_level").GetUInt32();
                            condition.TargetNum = jTask.GetProperty("enemy_count").GetUInt32();
                        }
                        else
                        {
                            if (!Enum.TryParse(jTask.GetProperty("item_id").GetString(), true, out ItemId itemId))
                            {
                                var name = jTask.GetProperty("item_id").GetString();
                                Logger.Error($"Failed to parse ItemId={name}. Skipping.");
                                continue;
                            }

                            condition.TargetId = (uint)itemId;
                        }

                        tasks[releaseLv].JobOrderProgressList.Add(condition);
                    }

                    foreach (var (_, task) in tasks)
                    {
                        asset.JobOrders[jobId][releaseType][releaseId].Add(task);
                    }
                }
            }

            return asset;
        }

        private static readonly Dictionary<string, EnemyId> UniqueNameReplacements = new Dictionary<string, EnemyId>()
        {
            ["CorpsePunisher"] = EnemyId.BoltCorpsePunisher,
            ["DamnedSlingGoblin"] = EnemyId.DamnedSlingGoblinFlask,
            ["EvilDragon"] = EnemyId.TheEvilDragon0,
            ["GreaterGoblin"] = EnemyId.GreaterGoblinSword,
            ["Ifrit"] = EnemyId.Ifrit2ndForm,
            ["PhantasmicGreatDragon"] = EnemyId.WhiteDragon,
            ["RogueGuardian"] = EnemyId.RogueDefender,
            ["SlingGoblin"] = EnemyId.SlingGoblinTorch,
            ["SlingHobgoblin"] = EnemyId.SlingHobgoblinTorch,
            ["Undead"] = EnemyId.UndeadMale,
            ["WarReadyGorecyclops"] = EnemyId.WarReadyGorecyclopsLightArmor0,
            ["WarReadyGoremanticore"] = EnemyId.WarReadyGoremanticoreLightArmor,
            ["WarReadyGrimwarg"] = EnemyId.WarReadyGrimwargLightArmor,
            ["WarReadyNightmare"] = EnemyId.WarReadyNightmareLightArmor,
            ["WarReadyOgre"] = EnemyId.WarReadyOgreLightArmor,
            ["WarReadySaurian"] = EnemyId.WarReadySaurianLightArmor,
        };
    }
}
