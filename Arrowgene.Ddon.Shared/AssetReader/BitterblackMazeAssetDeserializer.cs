using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using System;
using static Arrowgene.Ddon.Shared.Csv.GmdCsv;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class BitterblackMazeAssetDeserializer : IAssetDeserializer<BitterblackMazeAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(BitterblackMazeAssetDeserializer));

        public BitterblackMazeAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            BitterblackMazeAsset asset = new BitterblackMazeAsset();

            string json = File.ReadAllText(path);
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

                asset.JobEquipment[jobId] = new List<Item>();
                for (int i = 0; i < 2; i++)
                {
                    asset.JobEquipment[jobId].Add(null);
                }
            }

            foreach (var itemId in document.RootElement.GetProperty("rare_item_appraisal_list").EnumerateArray())
            {
                asset.RareItemAppraisalList.Add(new CDataCommonU32() { Value = itemId.GetUInt32() });
            }

            foreach (var itemId in document.RootElement.GetProperty("item_takeaway_list").EnumerateArray())
            {
                asset.ItemTakeawayList.Add(new CDataCommonU32() { Value = itemId.GetUInt32() });
            }

            return asset;
        }

        private StageId ParseStageId(JsonElement jStageId)
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

            return new StageId(id, layerNo, groupId);
        }
    }
}

