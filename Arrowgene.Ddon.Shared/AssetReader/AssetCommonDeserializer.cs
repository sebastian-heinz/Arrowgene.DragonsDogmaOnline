using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
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

        public bool ParseEnemyGroups(QuestDropItemAsset questDrops, Dictionary<uint, QuestEnemyGroup> EnemyGroups, JsonElement jElement)
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
                    GroupId = groupId
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

                foreach (var enemy in jGroup.GetProperty("enemies").EnumerateArray())
                {
                    bool isBoss = false;
                    if (enemy.TryGetProperty("is_boss", out JsonElement jIsBoss))
                    {
                        isBoss = jIsBoss.GetBoolean();
                    }

                    byte index = 0;
                    if (enemyGroup.PlacementType == QuestEnemyPlacementType.Manual)
                    {
                        if (!enemy.TryGetProperty("index", out JsonElement jEnemyIndex))
                        {
                            Logger.Error($"Manual placed enemy group requires an index value. Unable to parse.");
                            return false;
                        }
                        index = jEnemyIndex.GetByte();
                    }

                    bool isRequired = true;
                    if (enemy.TryGetProperty("is_required", out JsonElement jIsRequired))
                    {
                        isRequired = jIsRequired.GetBoolean();
                    }

                    uint repopWaitSecond = 0;
                    if (enemy.TryGetProperty("repop_wait_second", out JsonElement jRepopWaitSecond))
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

                    if (enemy.TryGetProperty("drop_items", out JsonElement itemsList))
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
                        EnemyId = Convert.ToUInt32(enemy.GetProperty("enemy_id").GetString(), 16),
                        Lv = enemy.GetProperty("level").GetUInt16(),
                        Experience = enemy.GetProperty("exp").GetUInt32(),
                        IsBossBGM = isBoss,
                        IsBossGauge = isBoss,
                        Scale = 100,
                        EnemyTargetTypesId = (byte)(isRequired ? 4 : 1),
                        Index = index,
                        IsRequired = isRequired,
                        RepopWaitSecond = repopWaitSecond,
                    };

                    ApplyOptionalEnemyKeys(enemy, questEnemy);

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

        private void ApplyOptionalEnemyKeys(JsonElement enemy, Enemy questEnemey)
        {
            if (enemy.TryGetProperty("pp", out JsonElement jPpAmount))
            {
                questEnemey.PPDrop = jPpAmount.GetUInt32();
            }

            if (enemy.TryGetProperty("named_enemy_params_id", out JsonElement jNamedEnemyParamsId))
            {
                questEnemey.NamedEnemyParams = NamedParams.GetValueOrDefault(jNamedEnemyParamsId.GetUInt32(), NamedParam.DEFAULT_NAMED_PARAM);
            }

            if (enemy.TryGetProperty("raid_boss_id", out JsonElement jRaidBossId))
            {
                questEnemey.RaidBossId = jRaidBossId.GetUInt32();
            }

            if (enemy.TryGetProperty("scale", out JsonElement jScale))
            {
                questEnemey.Scale = jScale.GetUInt16();
            }

            if (enemy.TryGetProperty("hm_present_no", out JsonElement jHmPresetNo))
            {
                questEnemey.HmPresetNo = jHmPresetNo.GetUInt16();
            }

            if (enemy.TryGetProperty("start_think_tbl_no", out JsonElement jStartThinkTblNo))
            {
                questEnemey.StartThinkTblNo = jStartThinkTblNo.GetByte();
            }

            if (enemy.TryGetProperty("repop_num", out JsonElement jRepopNum))
            {
                questEnemey.RepopNum = jRepopNum.GetByte();
            }

            if (enemy.TryGetProperty("repop_count", out JsonElement jRepopCount))
            {
                questEnemey.RepopCount = jRepopCount.GetByte();
            }

            if (enemy.TryGetProperty("enemy_target_types_id", out JsonElement jEnemyTargetTypesId))
            {
                questEnemey.EnemyTargetTypesId = jEnemyTargetTypesId.GetByte();
            }

            if (enemy.TryGetProperty("montage_fix_no", out JsonElement jMontageFixNo))
            {
                questEnemey.MontageFixNo = jMontageFixNo.GetByte();
            }

            if (enemy.TryGetProperty("set_type", out JsonElement jSetType))
            {
                questEnemey.SetType = jSetType.GetByte();
            }

            if (enemy.TryGetProperty("infection_type", out JsonElement jInfectionType))
            {
                questEnemey.InfectionType = jInfectionType.GetByte();
            }

            if (enemy.TryGetProperty("is_boss_gauge", out JsonElement jIsBossGauge))
            {
                questEnemey.IsBossGauge = jIsBossGauge.GetBoolean();
            }

            if (enemy.TryGetProperty("is_boss_bgm", out JsonElement jIsBossBGM))
            {
                questEnemey.IsBossBGM = jIsBossBGM.GetBoolean();
            }

            if (enemy.TryGetProperty("is_manual_set", out JsonElement jIsManualSet))
            {
                questEnemey.IsManualSet = jIsManualSet.GetBoolean();
            }

            if (enemy.TryGetProperty("is_area_boss", out JsonElement jIsAreaBoss))
            {
                questEnemey.IsAreaBoss = jIsAreaBoss.GetBoolean();
            }

            if (enemy.TryGetProperty("blood_orbs", out JsonElement jBloodOrbs))
            {
                questEnemey.BloodOrbs = jBloodOrbs.GetUInt32();
            }

            if (enemy.TryGetProperty("high_orbs", out JsonElement jHighOrbs))
            {
                questEnemey.HighOrbs = jHighOrbs.GetUInt32();
            }

            if (enemy.TryGetProperty("spawn_time_start", out JsonElement jSpawnTimeStart))
            {
                questEnemey.SpawnTimeStart = jSpawnTimeStart.GetUInt32();
            }

            if (enemy.TryGetProperty("spawn_time_end", out JsonElement jSpawnTimeEnd))
            {
                questEnemey.SpawnTimeEnd = jSpawnTimeEnd.GetUInt32();
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
    }
}
