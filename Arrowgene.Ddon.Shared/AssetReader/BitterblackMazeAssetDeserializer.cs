using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;

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
                foreach (var mazeTier in config.Value.EnumerateArray())
                {
                    var stageId = mazeTier.GetProperty("stage_id");
                    BitterblackMazeConfig configElement = new BitterblackMazeConfig()
                    {
                        Tier = mazeTier.GetProperty("tier").GetByte(),
                        ContentName = mazeTier.GetProperty("name").GetString(),
                        StageId = ParseStageId(stageId)
                    };

                    foreach (var dest in mazeTier.GetProperty("destinations").EnumerateArray())
                    {
                        configElement.Destinations.Add(dest.GetUInt32());
                    }

                    asset.Stages[configElement.StageId] = configElement;
                }
            }

            var jAppraisals = document.RootElement.GetProperty("appraisals").EnumerateArray();
            foreach (var jAppraisal in jAppraisals)
            {
                BitterblackMazeAppraisalItem item = new BitterblackMazeAppraisalItem()
                {
                    ItemId = jAppraisal.GetProperty("item_id").GetUInt32(),
                    Name = jAppraisal.GetProperty("name").GetString()
                };

                foreach (var jLotItem in jAppraisal.GetProperty("pool").EnumerateArray())
                {
                    BitterblackAppraisalLotteryItem lotItem = new BitterblackAppraisalLotteryItem()
                    {
                        ItemId = jLotItem.GetProperty("item_id").GetUInt32(),
                        Name = jLotItem.GetProperty("name").GetString(),
                        Amount = jLotItem.GetProperty("amount").GetUInt32()
                    };
                    item.LootPool.Add(lotItem);
                }

                asset.Appraisals.Add(item);
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

