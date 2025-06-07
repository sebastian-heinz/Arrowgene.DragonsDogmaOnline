using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Asset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class LightQuestAssetDeserializer : IAssetDeserializer<LightQuestAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(LightQuestAssetDeserializer));

        public LightQuestAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            LightQuestAsset asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var enemySpawnAssets = document.RootElement.GetProperty("lestania_enemy_nodes").EnumerateArray().ToList();
            foreach (var jEnemyAsset in enemySpawnAssets)
            {
                var jQuestAreaId = jEnemyAsset.GetProperty("quest_area_id");
                var questAreaId = Enum.Parse<QuestAreaId>(jQuestAreaId.GetString(), true);
                var nodes = jEnemyAsset.GetProperty("nodes").EnumerateArray().Select(x => x.GetUInt32()).ToHashSet();

                asset.LestaniaEnemyNodes[questAreaId] = nodes;
            }

            var gatheringAssets = document.RootElement.GetProperty("lestania_gathering_nodes").EnumerateArray().ToList();
            foreach (var jGatheringAsset in gatheringAssets)
            {
                var jQuestAreaId = jGatheringAsset.GetProperty("quest_area_id");
                var questAreaId = Enum.Parse<QuestAreaId>(jQuestAreaId.GetString(), true);
                var nodes = jGatheringAsset.GetProperty("nodes").EnumerateArray().Select(x => x.GetUInt32()).ToHashSet();

                asset.LestaniaGatheringNodes[questAreaId] = nodes;
            }

            var generatingAssets = document.RootElement.GetProperty("quest_generating_groups").EnumerateArray();
            foreach (var jGeneratingAsset in generatingAssets)
            {
                LightQuestGeneratingAsset generatingAsset = new();
                generatingAsset.Name = jGeneratingAsset.GetProperty("name").ToString();
                generatingAsset.BoardId = Enum.Parse<QuestBoardBaseId>(jGeneratingAsset.GetProperty("board_id").ToString(), true);
                generatingAsset.Type = Enum.Parse<LightQuestType>(jGeneratingAsset.GetProperty("type").ToString(), true);

                if (jGeneratingAsset.TryGetProperty("allow_normal_quests", out var jNormalQuests))
                {
                    generatingAsset.AllowNormalQuests = jNormalQuests.GetBoolean();
                }

                if (jGeneratingAsset.TryGetProperty("allow_area_orders", out var jAreaOrders))
                {
                    generatingAsset.AllowAreaOrders = jAreaOrders.GetBoolean();
                }

                if (jGeneratingAsset.TryGetProperty("bias_lower", out var jBiasDir))
                {
                    generatingAsset.BiasLower = jBiasDir.GetBoolean();
                }
                if (jGeneratingAsset.TryGetProperty("bias_rerolls", out var jBiasCount))
                {
                    generatingAsset.BiasRerolls = jBiasCount.GetUInt32();
                }

                if (jGeneratingAsset.TryGetProperty("min_quests", out var jMinQuests))
                {
                    generatingAsset.MinQuests = jMinQuests.GetInt32();
                }
                if (jGeneratingAsset.TryGetProperty("max_quests", out var jMaxQuests))
                {
                    generatingAsset.MaxQuests = jMaxQuests.GetInt32();
                }
                if (jGeneratingAsset.TryGetProperty("min_count", out var jMinCount))
                {
                    generatingAsset.MinCount = jMinCount.GetInt32();
                }
                if (jGeneratingAsset.TryGetProperty("max_count", out var jMaxCount))
                {
                    generatingAsset.MaxCount = jMaxCount.GetInt32();
                }
                if (jGeneratingAsset.TryGetProperty("min_level", out var jMinLevel))
                {
                    generatingAsset.MinLevel = jMinLevel.GetUInt32();
                }
                if (jGeneratingAsset.TryGetProperty("max_level", out var jMaxLevel))
                {
                    generatingAsset.MaxLevel = jMaxLevel.GetUInt32();
                }

                asset.GeneratingAssets.Add(generatingAsset);
            }

            return asset;
        }
    }
}
