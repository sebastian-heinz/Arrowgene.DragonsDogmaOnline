using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class AssetCommonDeserializer
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AssetCommonDeserializer));

        private Dictionary<uint, NamedParam> NamedParams;

        public AssetCommonDeserializer(Dictionary<uint, NamedParam> namedParams)
        {
            NamedParams = namedParams;
        }

        public bool ParseEnemyGroups(uint questScheduleId, QuestDropItemAsset questDrops, Dictionary<uint, QuestEnemyGroup> EnemyGroups, JsonElement jElement)
        {
            if (!jElement.TryGetProperty("enemy_groups", out JsonElement jGroups))
            {
                // No Enemy groups to parse
                return true;
            }

            uint groupId = 0;
            foreach (var jGroup in jGroups.EnumerateArray())
            {
                QuestEnemyGroup enemyGroup = new QuestEnemyGroup()
                {
                    GroupId = groupId,
                };

                if (!jGroup.TryGetProperty("stage_id", out JsonElement jStageId))
                {
                    Logger.Info("Required stage_id field for enemy group not found.");
                    return false;
                }

                enemyGroup.StageLayoutId = ParseStageId(jStageId);

                enemyGroup.SubGroupId = 0;
                if (jGroup.TryGetProperty("subgroup_id", out JsonElement jSubGroupId))
                {
                    enemyGroup.SubGroupId = jSubGroupId.GetByte();
                }

                enemyGroup.StartingIndex = 0;
                if (jGroup.TryGetProperty("starting_index", out JsonElement jStartingIndex))
                {
                    enemyGroup.StartingIndex = jStartingIndex.GetUInt32();
                }

                enemyGroup.PlacementType = QuestEnemyPlacementType.Automatic;
                if (jGroup.TryGetProperty("placement_type", out JsonElement jPlacementType))
                {
                    if (!Enum.TryParse(jPlacementType.GetString(), true, out QuestEnemyPlacementType placementType))
                    {
                        Logger.Error($"Invalid Quest Enemy Placement Type");
                        return false;
                    }

                    enemyGroup.PlacementType = placementType;
                }

                for (int i = 0; i < jGroup.GetProperty("enemies").EnumerateArray().Count(); i++)
                {
                    var jEnemy = jGroup.GetProperty("enemies")[i];

                    bool isBoss = false;
                    if (jEnemy.TryGetProperty("is_boss", out JsonElement jIsBoss))
                    {
                        isBoss = jIsBoss.GetBoolean();
                    }

                    byte index = 0;
                    if (enemyGroup.PlacementType == QuestEnemyPlacementType.Manual)
                    {
                        if (!jEnemy.TryGetProperty("index", out JsonElement jEnemyIndex))
                        {
                            Logger.Error($"Manual placed enemy group requires an index value. Unable to parse.");
                            return false;
                        }
                        index = jEnemyIndex.GetByte();
                    }
                    else
                    {
                        index = (byte)(enemyGroup.StartingIndex + i);
                    }

                    bool isRequired = true;
                    if (jEnemy.TryGetProperty("is_required", out JsonElement jIsRequired))
                    {
                        isRequired = jIsRequired.GetBoolean();
                    }

                    uint repopWaitSecond = 0;
                    if (jEnemy.TryGetProperty("repop_wait_second", out JsonElement jRepopWaitSecond))
                    {
                        repopWaitSecond = jRepopWaitSecond.GetUInt32();
                    }

                    // Look for custom drops here
                    bool customDropItems = false;

                    // Setting default values in case a custom table is defined.
                    DropsTable customTable = new()
                    {
                        Id = 0,
                        MdlType = 0,
                    };

                    if (jEnemy.TryGetProperty("drop_items", out JsonElement itemsList))
                    {
                        customDropItems = true;
                        var list = itemsList.EnumerateArray();

                        foreach (var items in list)
                        {
                            GatheringItem dropItems = new()
                            {
                                ItemId = AssetCommonDeserializer.ParseItemId(items.GetProperty("item_id")),
                                ItemNum = items.GetProperty("item_min").GetUInt32(),
                                MaxItemNum = items.GetProperty("item_max").GetUInt32(),
                                Quality = items.GetProperty("quality").GetUInt32(),
                                IsHidden = false,
                                DropChance = items.GetProperty("drop_chance").GetDouble()
                            };

                            customTable.Items.Add(dropItems);
                        }

                    }

                    var questEnemy = new InstancedEnemy()
                    {
                        EnemyId = Convert.ToUInt32(jEnemy.GetProperty("enemy_id").GetString(), 16),
                        Lv = jEnemy.GetProperty("level").GetUInt16(),
                        IsBossBGM = isBoss,
                        IsBossGauge = isBoss,
                        Scale = 100,
                        EnemyTargetTypesId = (byte)(isRequired ? 4 : 1),
                        Index = index,
                        IsRequired = isRequired,
                        RepopWaitSecond = repopWaitSecond,
                        QuestScheduleId = questScheduleId,
                    };

                    ApplyOptionalEnemyKeys(jEnemy, questEnemy);

                    if (customDropItems)
                    {
                        questEnemy.DropsTable = customTable;
                    }
                    else
                    {
                        questEnemy.DropsTable = questDrops.GetDropTable(questEnemy.EnemyId, questEnemy.Lv);
                    }

                    enemyGroup.Enemies.Add(questEnemy);
                }

                EnemyGroups[groupId++] = enemyGroup;
            }

            return true;
        }

        private void ApplyOptionalEnemyKeys(JsonElement enemy, InstancedEnemy questEnemy)
        {
            if (enemy.TryGetProperty("pp", out JsonElement jPpAmount))
            {
                questEnemy.PPDrop = jPpAmount.GetUInt32();
            }

            if (enemy.TryGetProperty("named_enemy_params_id", out JsonElement jNamedEnemyParamsId))
            {
                questEnemy.NamedEnemyParams = NamedParams.GetValueOrDefault(jNamedEnemyParamsId.GetUInt32(), NamedParam.DEFAULT_NAMED_PARAM);
            }

            if (enemy.TryGetProperty("raid_boss_id", out JsonElement jRaidBossId))
            {
                questEnemy.RaidBossId = jRaidBossId.GetUInt32();
            }

            if (enemy.TryGetProperty("scale", out JsonElement jScale))
            {
                questEnemy.Scale = jScale.GetUInt16();
            }

            if (enemy.TryGetProperty("hm_present_no", out JsonElement jHmPresetNo))
            {
                questEnemy.HmPresetNo = jHmPresetNo.GetUInt16();
            }

            if (enemy.TryGetProperty("start_think_tbl_no", out JsonElement jStartThinkTblNo))
            {
                questEnemy.StartThinkTblNo = jStartThinkTblNo.GetByte();
            }

            if (enemy.TryGetProperty("repop_num", out JsonElement jRepopNum))
            {
                questEnemy.RepopNum = jRepopNum.GetByte();
            }

            if (enemy.TryGetProperty("repop_count", out JsonElement jRepopCount))
            {
                questEnemy.RepopCount = jRepopCount.GetByte();
            }

            if (enemy.TryGetProperty("enemy_target_types_id", out JsonElement jEnemyTargetTypesId))
            {
                questEnemy.EnemyTargetTypesId = jEnemyTargetTypesId.GetByte();
            }

            if (enemy.TryGetProperty("montage_fix_no", out JsonElement jMontageFixNo))
            {
                questEnemy.MontageFixNo = jMontageFixNo.GetByte();
            }

            if (enemy.TryGetProperty("set_type", out JsonElement jSetType))
            {
                questEnemy.SetType = jSetType.GetByte();
            }

            if (enemy.TryGetProperty("infection_type", out JsonElement jInfectionType))
            {
                questEnemy.InfectionType = jInfectionType.GetByte();
            }

            if (enemy.TryGetProperty("is_boss_gauge", out JsonElement jIsBossGauge))
            {
                questEnemy.IsBossGauge = jIsBossGauge.GetBoolean();
            }

            if (enemy.TryGetProperty("is_boss_bgm", out JsonElement jIsBossBGM))
            {
                questEnemy.IsBossBGM = jIsBossBGM.GetBoolean();
            }

            if (enemy.TryGetProperty("is_manual_set", out JsonElement jIsManualSet))
            {
                questEnemy.IsManualSet = jIsManualSet.GetBoolean();
            }

            if (enemy.TryGetProperty("is_area_boss", out JsonElement jIsAreaBoss))
            {
                questEnemy.IsAreaBoss = jIsAreaBoss.GetBoolean();
            }

            if (enemy.TryGetProperty("blood_orbs", out JsonElement jBloodOrbs))
            {
                questEnemy.BloodOrbs = jBloodOrbs.GetUInt32();
            }

            if (enemy.TryGetProperty("high_orbs", out JsonElement jHighOrbs))
            {
                questEnemy.HighOrbs = jHighOrbs.GetUInt32();
            }

            if (enemy.TryGetProperty("spawn_time_start", out JsonElement jSpawnTimeStart))
            {
                questEnemy.SpawnTimeStart = jSpawnTimeStart.GetUInt32();
            }

            if (enemy.TryGetProperty("spawn_time_end", out JsonElement jSpawnTimeEnd))
            {
                questEnemy.SpawnTimeEnd = jSpawnTimeEnd.GetUInt32();
            }

            if (enemy.TryGetProperty("exp_scheme", out JsonElement jExpScheme))
            {
                if (Enum.TryParse(jExpScheme.GetString(), true, out EnemyExpScheme parsedScheme))
                {
                    questEnemy.ExpScheme = parsedScheme;
                }
                else
                {
                    Logger.Error($"Invalid exp scheme {jExpScheme}");
                }
            }

            if (questEnemy.ExpScheme != EnemyExpScheme.Automatic && questEnemy.ExpScheme != EnemyExpScheme.Exm)
            {
                if (!enemy.TryGetProperty("exp", out JsonElement jExperience))
                {
                    Logger.Error("Quest enemy has no exp scheme type, but no exp value was assigned.");
                    return;
                }
                questEnemy.Experience = jExperience.GetUInt32();
            }
        }

        public static StageLayoutId ParseStageId(JsonElement jStageId)
        {
            uint id = jStageId.GetProperty("id").GetUInt32();

            byte layerNo = 0;
            if (jStageId.TryGetProperty("layer_no", out JsonElement jLayerNo))
            {
                layerNo = jLayerNo.GetByte();
            }

            uint groupId = 0;
            if (jStageId.TryGetProperty("group_id", out JsonElement jGroupId))
            {
                groupId = jGroupId.GetUInt32();
            }

            return new StageLayoutId(id, layerNo, groupId);
        }

        public static ItemId ParseItemId(JsonElement jItemId)
        {
            ItemId result = ItemId.HealingPotion;

            if (jItemId.ValueKind == JsonValueKind.Number)
            {
                result = (ItemId) jItemId.GetUInt32();
            }
            else if (jItemId.ValueKind == JsonValueKind.String)
            {
                if (!Enum.TryParse(jItemId.GetString(), true, out result))
                {
                    throw new Exception("Failed to parse item reward. Skipping.");
                }
            }

            return result;
        }


        public static (long, long) ConvertTimeToMilliseconds(string SpawnTime)
        {
            // this converts the time (07:00,18:00) as example, down into milliseconds, via splitting into hours/minutes and then combining each respect time into start/end
            // Split the spawnTime string at the comma to get start and end times
            string[] spawnTimes = SpawnTime.Split(',');

            // Split the start time at the colon to get hours and minutes
            string[] startTimeComponents = spawnTimes[0].Split(':');
            int startHours = int.Parse(startTimeComponents[0]);
            int startMinutes = int.Parse(startTimeComponents[1]);

            // Split the end time at the colon to get hours and minutes
            string[] endTimeComponents = spawnTimes[1].Split(':');
            int endHours = int.Parse(endTimeComponents[0]);
            int endMinutes = int.Parse(endTimeComponents[1]);

            // Convert hours and minutes into milliseconds
            return ((startHours * 3600000) + (startMinutes * 60000), (endHours * 3600000) + (endMinutes * 60000));
        }
    }
}
