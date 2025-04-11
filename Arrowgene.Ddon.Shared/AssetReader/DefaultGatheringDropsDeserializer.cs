using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class DefaultGatheringDropsDeserializer : IAssetDeserializer<DefaultGatheringDropsAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(DefaultGatheringDropsDeserializer));

        public DefaultGatheringDropsAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            DefaultGatheringDropsAsset asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var jAreaId in document.RootElement.EnumerateObject())
            {
                
                if (!Enum.TryParse(jAreaId.Name, true, out QuestAreaId areaId))
                {
                    Logger.Error($"Invalid AreaId={jAreaId.Name}");
                    continue;
                }

                if (!asset.AreaDefaultDrops.ContainsKey(areaId))
                {
                    asset.AreaDefaultDrops[areaId] = new();

                    foreach (DropCategory category in Enum.GetValues(typeof(DropCategory)))
                    {
                        asset.AreaDefaultDrops[areaId][category] = new();
                    }
                }

                foreach (var jDrop in jAreaId.Value.EnumerateArray())
                {
                    if (!Enum.TryParse(jDrop.GetProperty("type").ToString(), true, out DropCategory dropCategory))
                    {
                        var msg = jDrop.GetProperty("type").ToString();
                        Logger.Error($"Invalid DropCategory={msg}");
                        continue;
                    }

                    var drop = new DefaultGatheringDrop()
                    {
                        LocationName = jDrop.GetProperty("location").GetString(),
                        IsSpot = jDrop.GetProperty("is_spot").GetBoolean(),
                        ItemId = (ItemId)jDrop.GetProperty("item_id").GetUInt32(),
                        AreaId = areaId,
                        StageId = jDrop.GetProperty("stage_id").GetUInt32(),
                        MinAreaRank = jDrop.GetProperty("area_rank_minimum").GetUInt32(),
                        DropCategory = dropCategory,
                        ItemLevel = jDrop.GetProperty("item_level").GetUInt32(),
                        Quality = jDrop.GetProperty("quality").GetUInt32(),
                        MinAmount = jDrop.GetProperty("min_amount").GetUInt32(),
                        MaxAmount = jDrop.GetProperty("max_amount").GetUInt32(),
                    };

                    if (drop.MinAmount == 0)
                    {
                        Logger.Error($"Invalid Minimum amount of '0' for '{drop.ItemId}: {drop.LocationName}'. The minimum amount should always be >= 1." +
                                      "The item already goes through a random selection process before being selected. Skipping.");
                        continue;
                    }

                    if (drop.MaxAmount != 0 && drop.MinAmount > drop.MaxAmount)
                    {
                        Logger.Error($"Invalid (Min,Max) constraint for '{drop.ItemId}: {drop.LocationName}'. The maximum must be >= the minimum. Skipping.");
                        continue;
                    }

                    if (drop.IsSpot)
                    {
                        drop.SpotPosId = jDrop.GetProperty("spot_pos_id").GetUInt32();
                        drop.SpotStageLayoutId = AssetCommonDeserializer.ParseStageId(jDrop.GetProperty("spot_stage_layout_id")); 
                    }

                    foreach (var jWeather in jDrop.GetProperty("weather").EnumerateArray())
                    {
                        if (!Enum.TryParse(jWeather.GetString(), true, out Weather weather))
                        {
                            Logger.Error($"Invalid Weather={jWeather.GetString()}");
                            continue;
                        }
                        drop.PresentDuringWeather.Add(weather);
                    }
                    drop.ValidPeriod = AssetCommonDeserializer.ConvertTimeToMilliseconds(jDrop.GetProperty("valid_period").ToString());

                    if (drop.IsSpot)
                    {
                        // asset.SpotDefaultDrops[(drop.SpotStageLayoutId, drop.SpotPosId)].Add(drop);
                    }
                    else
                    {
                        asset.AreaDefaultDrops[areaId][dropCategory].Add(drop);
                    }
                }
                
            }

            return asset;
        }
    }
}
