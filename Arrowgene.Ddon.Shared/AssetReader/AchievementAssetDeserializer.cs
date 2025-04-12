using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class AchievementAssetDeserializer : IAssetDeserializer<Dictionary<(AchievementType, uint), List<AchievementAsset>>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AchievementAssetDeserializer));

        public Dictionary<(AchievementType, uint), List<AchievementAsset>> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            Dictionary<(AchievementType, uint), List<AchievementAsset>> asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var achievementAssets = document.RootElement.GetProperty("achievement_data").EnumerateArray().ToList();
            foreach (var jAchievement in achievementAssets)
            {
                var achievement = new AchievementAsset
                {
                    Category = (AchievementCategory)jAchievement.GetProperty("category").GetUInt32(),
                    Id = jAchievement.GetProperty("id").GetUInt32(),
                    SortId = jAchievement.GetProperty("sort_id").GetUInt32(),
                    Type = (AchievementType)Enum.Parse(typeof(AchievementType), jAchievement.GetProperty("type").GetString(), true),
                    Count = jAchievement.GetProperty("count").GetUInt32()
                };

                var jParam = jAchievement.GetProperty("param");
                switch (achievement.Type)
                {
                    case AchievementType.ClearBBM:
                        achievement.Param = (uint) Enum.Parse(typeof(AchievementBBMParam), jParam.GetString(), true);
                        break;
                    case AchievementType.CollectType:
                        achievement.Param = (uint)Enum.Parse(typeof(AchievementCollectParam), jParam.GetString(), true);
                        break;
                    case AchievementType.CraftType:
                    case AchievementType.CraftTypeUnique:
                        achievement.Param = (uint)Enum.Parse(typeof(AchievementCraftTypeParam), jParam.GetString(), true);
                        break;
                    case AchievementType.KillEnemyType:
                        achievement.Param = (uint)Enum.Parse(typeof(AchievementEnemyParam), jParam.GetString(), true);
                        break;
                    case AchievementType.MainLevel:
                    case AchievementType.PawnLevel:
                        achievement.Param = (byte)Enum.Parse(typeof(JobId), jParam.GetString(), true);
                        break;
                    case AchievementType.MainLevelGroup:
                        achievement.Param = (uint)Enum.Parse(typeof(AchievementLevelGroupParam), jParam.GetString(), true);
                        break;
                    case AchievementType.OrbDevote:
                        achievement.Param = (byte)Enum.Parse(typeof(OrbGainParamType), jParam.GetString(), true);
                        break;
                    case AchievementType.SparkleCollect:
                        achievement.Param = (uint)Enum.Parse(typeof(QuestAreaId), jParam.GetString(), true);
                        break;
                    case AchievementType.ClearQuestType:
                        achievement.Param = (uint)Enum.Parse(typeof(AchievementQuestTypeParam), jParam.GetString(), true);
                        break;
                    case AchievementType.ClearQuest:
                    case AchievementType.ClearSubstory:
                    case AchievementType.EnhanceItem:
                        achievement.Param = jParam.GetUInt32();
                        break;
                    default:
                        achievement.Param = 0;
                        break;
                }

                if(jAchievement.TryGetProperty("reward", out var jReward))
                {
                    achievement.RewardId = jReward.GetUInt32();
                }

                var key = (achievement.Type, achievement.Param);
                if (!asset.ContainsKey(key))
                {
                    asset[key] = new();
                }
                asset[key].Add(achievement);
            }

            foreach (var key in asset.Keys)
            {
                asset[key] = asset[key].OrderBy(x => x.Count).ToList();
            }

            return asset;
        }
    }
}
