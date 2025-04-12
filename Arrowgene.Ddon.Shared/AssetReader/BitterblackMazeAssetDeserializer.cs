using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static Arrowgene.Ddon.Shared.Asset.BitterblackMazeAsset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class BitterblackMazeAssetDeserializer : IAssetDeserializer<BitterblackMazeAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(BitterblackMazeAssetDeserializer));

        public BitterblackMazeAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            BitterblackMazeAsset asset = new BitterblackMazeAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var configurations = document.RootElement.GetProperty("maze_configurations").EnumerateObject();
            foreach (var config in configurations)
            {
                if (!Enum.TryParse(config.Name, true, out BattleContentMode contentMode))
                {
                    Logger.Error($"The battle content mode '{config.Name}' is not recognized. Unable to parse.");
                    return null;
                }

                foreach (var mazeTier in config.Value.EnumerateArray())
                {
                    var stageId = mazeTier.GetProperty("stage_id");
                    BitterblackMazeConfig configElement = new BitterblackMazeConfig()
                    {
                        ContentId = mazeTier.GetProperty("content_id").GetUInt32(),
                        Tier = mazeTier.GetProperty("tier").GetByte(),
                        ContentName = mazeTier.GetProperty("name").GetString(),
                        StageId = ParseStageId(stageId),
                        ContentMode = contentMode
                    };

                    foreach (var dest in mazeTier.GetProperty("destinations").EnumerateArray())
                    {
                        configElement.Destinations.Add(dest.GetUInt32());
                    }

                    asset.StageProgressionList.Add(new CDataBattleContentStageProgression()
                    {
                        Id = configElement.ContentId,
                        Tier = configElement.Tier,
                        ConnectionList = mazeTier.GetProperty("next_content_ids").EnumerateArray().Select(x => new CDataCommonU32() { Value = x.GetUInt32() }).ToList()
                    });

                    asset.Stages[configElement.StageId] = configElement;
                }
            }

            var lootRanges = document.RootElement.GetProperty("loot_ranges").EnumerateArray();
            foreach (var lootRange in lootRanges)
            {
                var record = new LootRange()
                {
                    NormalRange = (lootRange.GetProperty("normal").GetProperty("min").GetUInt32(), lootRange.GetProperty("normal").GetProperty("max").GetUInt32()),
                    SealedRange = (lootRange.GetProperty("sealed").GetProperty("min").GetUInt32(), lootRange.GetProperty("sealed").GetProperty("max").GetUInt32()),
                    JewelryChance  = lootRange.GetProperty("jewelry_chance").GetDouble(),
                    RareChance = lootRange.GetProperty("rare_chance").GetDouble(),
                    Marks = (lootRange.GetProperty("marks").GetProperty("gold").GetUInt32(), lootRange.GetProperty("marks").GetProperty("silver").GetUInt32(), lootRange.GetProperty("marks").GetProperty("red").GetUInt32())
                };

                foreach (var id in lootRange.GetProperty("stage_ids").EnumerateArray())
                {
                    asset.LootRanges[id.GetUInt32()] = record;
                }
            }

            // Fill up Null items for JobId.None
            asset.StarterEquipment[JobId.None] = new Dictionary<EquipType, List<Item?>>();
            asset.StarterEquipment[JobId.None][EquipType.Performance] = Enumerable.Repeat((Item) null, 15).ToList();
            asset.StarterEquipment[JobId.None][EquipType.Visual] = Enumerable.Repeat((Item) null, 15).ToList();
            asset.StarterJobEquipment[JobId.None] = Enumerable.Repeat((Item) null, 2).ToList();

            var starterEquipment = document.RootElement.GetProperty("starter_equipment").EnumerateArray();
            foreach (var jEquipmentSet in starterEquipment)
            {
                if (!Enum.TryParse(jEquipmentSet.GetProperty("job").GetString(), true, out JobId jobId))
                {
                    return null;
                }

                asset.StarterEquipment[jobId] = new Dictionary<EquipType, List<Item?>>();
                asset.StarterEquipment[jobId][EquipType.Performance] = new List<Item?>();
                asset.StarterEquipment[jobId][EquipType.Visual] = new List<Item?>();
                foreach (var equipment in  jEquipmentSet.GetProperty("equipment").EnumerateArray())
                {
                    uint itemId = equipment.GetProperty("item_id").GetUInt32();
                    var item = (itemId == 0) ? null : new Item() { ItemId = itemId };
                    asset.StarterEquipment[jobId][EquipType.Performance].Add(item);
                }

                // Fill up a bunch of null items for visual items in BBM
                foreach (var _ in asset.StarterEquipment[jobId][EquipType.Performance])
                {
                    asset.StarterEquipment[jobId][EquipType.Visual].Add(null);
                }

                asset.StarterJobEquipment[jobId] = new List<Item>();
                for (int i = 0; i < 2; i++)
                {
                    asset.StarterJobEquipment[jobId].Add(null);
                }
            }
            
            var starterJobItems = document.RootElement.GetProperty("starter_job_items").EnumerateArray();
            foreach(var jobItem in starterJobItems) 
            {
                uint itemId = jobItem.GetProperty("item_id").GetUInt32();
                uint itemQuantity = jobItem.GetProperty("quantity").GetUInt32();
                asset.StarterJobItems.Add((itemId, itemQuantity));
            }

            foreach (var itemId in document.RootElement.GetProperty("rare_item_appraisal_list").EnumerateArray())
            {
                asset.RareItemAppraisalList.Add(new CDataCommonU32() { Value = itemId.GetUInt32() });
            }

            foreach (var itemId in document.RootElement.GetProperty("item_takeaway_list").EnumerateArray())
            {
                asset.ItemTakeawayList.Add(new CDataCommonU32() { Value = itemId.GetUInt32() });
            }

            var jArmorLoot = document.RootElement.GetProperty("chest_loot").GetProperty("armor");
            foreach (var quality in jArmorLoot.EnumerateObject())
            {
                foreach (var equipmentCategory in quality.Value.EnumerateObject())
                {
                    if (!Enum.TryParse(equipmentCategory.Name, true, out BitterblackMazeEquipmentClass equipmentClass))
                    {
                        Logger.Error($"Unexpected category {equipmentCategory.Name}");
                        return null;
                    }

                    Dictionary<BitterblackMazeEquipmentClass, List<uint>> items = quality.NameEquals("low_quality") ? asset.LowQualityArmors : asset.HighQualityArmors;

                    if (!items.ContainsKey(equipmentClass))
                    {
                        items[equipmentClass] = new List<uint>();
                    }

                    foreach (var itemId in equipmentCategory.Value.EnumerateArray())
                    {
                        items[equipmentClass].Add(itemId.GetUInt32());
                    }
                }
            }

            var jWeaponLoot = document.RootElement.GetProperty("chest_loot").GetProperty("weapons");
            foreach (var quality in jWeaponLoot.EnumerateObject())
            {
                foreach (var equipmentCategory in quality.Value.EnumerateObject())
                {
                    if (!Enum.TryParse(equipmentCategory.Name, true, out JobId jobId))
                    {
                        Logger.Error($"Unexpected JobId {equipmentCategory.Name}");
                        return null;
                    }

                    Dictionary<JobId, List<uint>> items = quality.NameEquals("low_quality") ? asset.LowQualityWeapons : asset.HighQualityWeapons;

                    if (!items.ContainsKey(jobId))
                    {
                        items[jobId] = new List<uint>();
                    }

                    foreach (var itemId in equipmentCategory.Value.EnumerateArray())
                    {
                        items[jobId].Add(itemId.GetUInt32());
                    }
                }
            }

            var jOtherLoot = document.RootElement.GetProperty("chest_loot").GetProperty("other");
            foreach (var quality in jOtherLoot.EnumerateObject())
            {
                List<uint> items = quality.NameEquals("low_quality") ? asset.LowQualityOther : asset.HighQualityOther;
                foreach (var itemId in quality.Value.EnumerateArray())
                {
                    items.Add(itemId.GetUInt32());
                }
            }

            var jRareLoot = document.RootElement.GetProperty("chest_loot").GetProperty("rare");
            foreach (var quality in jRareLoot.EnumerateObject())
            {
                List<uint> items = quality.NameEquals("rotunda") ? asset.RotundaRare : asset.AbyssRare;
                foreach (var itemId in quality.Value.EnumerateArray())
                {
                    items.Add(itemId.GetUInt32());
                }
            }

            var jTrashLoot = document.RootElement.GetProperty("chest_loot").GetProperty("trash");
            foreach (var drop in jTrashLoot.EnumerateArray())
            {
                asset.ChestTrash.Add((drop.GetProperty("id").GetUInt32(), drop.GetProperty("max_amount").GetUInt32()));
            }

            return asset;
        }

        private StageLayoutId ParseStageId(JsonElement jStageId)
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
    }
}

