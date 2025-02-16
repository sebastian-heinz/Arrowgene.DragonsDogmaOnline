using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class EpitaphRoadAssertDeserializer : IAssetDeserializer<EpitaphRoadAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(EpitaphRoadAssertDeserializer));

        public EpitaphRoadAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            EpitaphRoadAsset asset = new EpitaphRoadAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var jEpiPath in document.RootElement.GetProperty("paths").EnumerateArray())
            {
                var epiPath = new EpitaphPath()
                {
                    Name = jEpiPath.GetProperty("name").GetString(),
                    DungeonId = jEpiPath.GetProperty("dungeon_id").GetUInt32(),
                    HubStageId = jEpiPath.GetProperty("hub_stage_id").GetUInt32(),
                    SoulItemId = jEpiPath.GetProperty("soul_item_id").GetUInt32(),
                    RewardBuffs = jEpiPath.GetProperty("reward_buffs").GetBoolean(),
                };

                if (!Enum.TryParse(jEpiPath.GetProperty("npc_id").GetString(), true, out epiPath.NpcId))
                {
                    Logger.Error("Unable to parse Epitaph Road NpcId. Skipping");
                    continue;
                }

                var finalTrialId = jEpiPath.GetProperty("final_trial_id");
                epiPath.FinalTrialId = (AssetCommonDeserializer.ParseStageId(finalTrialId.GetProperty("stage_id")), finalTrialId.GetProperty("pos_id").GetUInt32());

                foreach (var jStageId in jEpiPath.GetProperty("stage_ids").EnumerateArray())
                {
                    epiPath.StageIds.Add(jStageId.GetUInt32());
                }

                foreach (var jBarrier in jEpiPath.GetProperty("barriers").EnumerateArray())
                {
                    BlockadeType type;
                    if (!Enum.TryParse(jBarrier.GetProperty("type").GetString(), true, out type))
                    {
                        Logger.Error($"Unable to parse barricade type for {epiPath.Name}");
                        continue;
                    }

                    var itemId = jBarrier.GetProperty("item_id").GetUInt32();
                    var amount = jBarrier.GetProperty("amount").GetUInt32();

                    EpitaphBarrier barrier = new EpitaphBarrier()
                    {
                        Name = jBarrier.GetProperty("name").GetString(),
                    };

                    var stageId = jBarrier.GetProperty("stage_id").GetUInt32();
                    var groupId = jBarrier.GetProperty("group_id").GetUInt32();
                    var location = new StageLayoutId(stageId, 0, groupId);
                    var posId = jBarrier.GetProperty("pos_id").GetUInt32();

                    barrier.StageId = location;
                    barrier.PosId = posId;
                    barrier.EpitaphId = EpitaphId.GenerateBarrierId(location, posId, 0);
                    
                    if (type == BlockadeType.NpcId)
                    {
                        if (!Enum.TryParse(jBarrier.GetProperty("npc_id").GetString(), true, out barrier.NpcId))
                        {
                            Logger.Error($"Unable to parse barricade NpcId for {epiPath.Name}");
                            continue;
                        }
                        asset.BarriersByNpcId[barrier.NpcId] = barrier;
                    }

                    barrier.UnlockCost.Add(new CDataSoulOrdealItem()
                    {
                        ItemId = itemId,
                        Num = (ushort) amount
                    });

                    epiPath.Barriers.Add(barrier);

                    asset.BarriersByOmId[barrier.StageId] = barrier;
                    asset.EpitaphObjects[barrier.EpitaphId] = barrier;
                }

                foreach (var jStatue in jEpiPath.GetProperty("statues").EnumerateArray())
                {
                    var statue = new EpitaphStatue()
                    {
                        StageId = new StageLayoutId(jStatue.GetProperty("stage_id").GetUInt32(), 0, jStatue.GetProperty("group_id").GetUInt32()),
                        PosId = jStatue.GetProperty("pos_id").GetUInt32()
                    };
                    statue.EpitaphId = EpitaphId.GenerateStatueId(statue.StageId, statue.PosId, 0);

                    epiPath.Statues.Add(statue);

                    asset.StatuesByOmId[(statue.StageId, statue.PosId)] = statue;
                    asset.EpitaphObjects[statue.EpitaphId] = statue;
                }

                foreach (var jDoor in jEpiPath.GetProperty("doors").EnumerateArray())
                {
                    var door = new EpitaphDoor()
                    {
                        StageId = AssetCommonDeserializer.ParseStageId(jDoor.GetProperty("stage_id")),
                        PosId = jDoor.GetProperty("pos_id").GetUInt32()
                    };
                    door.EpitaphId = EpitaphId.GenerateDoorId(door.StageId, door.PosId, 0);

                    var jGatherPoints = jDoor.GetProperty("gathering_points");
                    var gatheringStageId = AssetCommonDeserializer.ParseStageId(jGatherPoints.GetProperty("stage_id"));
                    var amount = jGatherPoints.GetProperty("amount").GetUInt32();
                    for (int pointId = 0; pointId < amount; pointId++)
                    {
                        var gatheringPoint = new EpitaphGatheringPoint()
                        {
                            StageId = gatheringStageId,
                            PosId = (uint) pointId,
                            Door = door
                        };
                        gatheringPoint.EpitaphId = EpitaphId.GenerateGatheringPointId(gatheringPoint.StageId, gatheringPoint.PosId, 0);

                        door.GatheringPoints.Add(gatheringPoint);
                        asset.GatheringPointsByOmId[(gatheringPoint.StageId, gatheringPoint.PosId)] = gatheringPoint;
                        asset.EpitaphObjects[gatheringPoint.EpitaphId] = gatheringPoint;
                    }

                    epiPath.Doors.Add(door);

                    asset.DoorsByOmId[(door.StageId, door.PosId)] = door;
                    asset.EpitaphObjects[door.EpitaphId] = door;
                }

                foreach (var jDropTable in jEpiPath.GetProperty("drop_tables").EnumerateArray())
                {
                    var dropTable = new EpitaphDropTable()
                    {
                        TableId = jDropTable.GetProperty("id").GetUInt32()
                    };

                    foreach (var jItem in jDropTable.GetProperty("items").EnumerateArray())
                    {
                        var rewardItem = EpitaphAssetCommonDeserializer.ParseReward(jItem);
                        if (rewardItem == null)
                        {
                            Logger.Error($"Unable to parse golden chest reward item for {epiPath.Name}. Skipping");
                            continue;
                        }
                        dropTable.Items.Add(rewardItem);
                    }

                    epiPath.DropTables[dropTable.TableId] = dropTable;
                }

                foreach (var jChests in jEpiPath.GetProperty("chests").EnumerateArray())
                {
                    EpitaphWeeklyReward.Type definitionType = EpitaphWeeklyReward.Type.Single;
                    if (jChests.TryGetProperty("type", out JsonElement jDefType))
                    {
                        if (!Enum.TryParse(jDefType.GetString(), true, out definitionType))
                        {
                            Logger.Error($"Failed to parse chest type for {epiPath.Name}. Skipping");
                            continue;
                        }
                    }

                    var dropTableIds = new List<uint>();
                    foreach (var jTableId in jChests.GetProperty("drop_tables").EnumerateArray())
                    {
                        dropTableIds.Add(jTableId.GetUInt32());
                    }

                    if (definitionType == EpitaphWeeklyReward.Type.Range)
                    {
                        uint amount = jChests.GetProperty("amount").GetUInt32();
                        for (uint posId = 0; posId < amount; posId++)
                        {
                            var reward = new EpitaphWeeklyReward()
                            {
                                StageId = AssetCommonDeserializer.ParseStageId(jChests.GetProperty("stage_id")),
                                PosId = posId
                            };
                            reward.EpitaphId = EpitaphId.GenerateWeeklyRewardId(reward.StageId, reward.PosId, 0);
                            reward.DropTablesIds = dropTableIds;

                            epiPath.Chests.Add(reward);

                            asset.ChestsByOmId[(reward.StageId, reward.PosId)] = reward;
                            asset.EpitaphObjects[reward.EpitaphId] = reward;
                        }
                    }
                    else
                    {
                        var reward = new EpitaphWeeklyReward()
                        {
                            StageId = AssetCommonDeserializer.ParseStageId(jChests.GetProperty("stage_id")),
                            PosId = jChests.GetProperty("pos_id").GetUInt32()
                        };
                        reward.EpitaphId = EpitaphId.GenerateWeeklyRewardId(reward.StageId, reward.PosId, 0);
                        reward.DropTablesIds = dropTableIds;

                        epiPath.Chests.Add(reward);

                        asset.ChestsByOmId[(reward.StageId, reward.PosId)] = reward;
                        asset.EpitaphObjects[reward.EpitaphId] = reward;
                    }
                }

                foreach (var jTrashRecord in jEpiPath.GetProperty("dungeon_trash").EnumerateArray())
                {
                    var stageId = jTrashRecord.GetProperty("stage_id").GetUInt32();
                    if (!asset.RandomLootByStageId.ContainsKey(stageId))
                    {
                        asset.RandomLootByStageId[stageId] = new List<EpitaphItemReward>();
                    }

                    foreach (var jItem in jTrashRecord.GetProperty("items").EnumerateArray())
                    {
                        asset.RandomLootByStageId[stageId].Add(EpitaphAssetCommonDeserializer.ParseReward(jItem));
                    }
                }

                uint i = 1;
                foreach (var jSection in jEpiPath.GetProperty("sections").EnumerateArray())
                {
                    var section = new EpitaphSection()
                    {
                        Name = jSection.GetProperty("name").GetString(),
                        StageId = jSection.GetProperty("stage_id").GetUInt32(),
                        StartingPos = jSection.GetProperty("start_pos").GetUInt32(),
                        DungeonId = epiPath.DungeonId
                    };
                    section.EpitaphId = EpitaphId.GeneratePathId(epiPath.NpcId, i++);

                    foreach (var jItem in jSection.GetProperty("unlock_cost").EnumerateArray())
                    {
                        var item = new CDataSoulOrdealItem()
                        {
                            ItemId = jItem.GetProperty("item_id").GetUInt32(),
                            Num = jItem.GetProperty("amount").GetUInt16(),
                        };
                        section.UnlockCost.Add(item);
                    }

                    foreach (var jOmId in jSection.GetProperty("barrier_oms").EnumerateArray())
                    {
                        section.BarrierOmIds.Add(jOmId.GetUInt32());
                    }

                    foreach (var jBarrierDependency in jSection.GetProperty("barrier_dependency").EnumerateArray())
                    {
                        var stageId = AssetCommonDeserializer.ParseStageId(jBarrierDependency.GetProperty("stage_id"));
                        var posId = jBarrierDependency.GetProperty("pos_id").GetUInt32();

                        var barrierId = EpitaphId.GenerateBarrierId(stageId, posId, 0);

                        var barrier = (EpitaphBarrier) asset.EpitaphObjects[barrierId];

                        barrier.DependentSectionIds.Add(section.EpitaphId);
                        section.BarrierDependencies.Add(barrierId);
                    }

                    // Array of epitaph ids required to be unlocked
                    foreach (var jUnlockDependency in jSection.GetProperty("unlock_dependency").EnumerateArray())
                    {
                        section.UnlockDependencies.Add(jUnlockDependency.GetUInt32());
                    }

                    epiPath.Sections.Add(section);

                    asset.EpitaphObjects[section.EpitaphId] = section;
                }

                if (!asset.Paths.ContainsKey(epiPath.DungeonId))
                {
                    asset.Paths[epiPath.DungeonId] = epiPath;
                }
            }

            foreach (var jBuff in document.RootElement.GetProperty("buffs").EnumerateArray())
            {
                var buff = new EpitaphBuff();
                if (!Enum.TryParse(jBuff.GetProperty("type").GetString(), true, out buff.Type))
                {
                    Logger.Error("Unable to parse Epitaph Road buff. Skipping");
                    continue;
                }

                buff.BuffId = jBuff.GetProperty("buff_id").GetUInt32();
                buff.BuffEffect = jBuff.GetProperty("buff_effect").GetUInt32();
                buff.Increment = jBuff.GetProperty("increment").GetUInt32();
                buff.Name = jBuff.GetProperty("name").GetString();

                if (!asset.BuffsByType.ContainsKey(buff.Type))
                {
                    asset.BuffsByType[buff.Type] = new List<EpitaphBuff>();
                }

                asset.BuffsByType[buff.Type].Add(buff);
                asset.BuffsById[buff.BuffId] = buff;
            }

            return asset;
        }
    }
}
