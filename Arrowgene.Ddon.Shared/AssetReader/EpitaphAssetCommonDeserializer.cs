using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;
using System;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class EpitaphAssetCommonDeserializer
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(EpitaphAssetCommonDeserializer));

        public static EpitaphItemReward ParseReward(JsonElement jItemReward)
        {
            EpitaphItemReward reward = new EpitaphItemReward();

            reward.Type = SoulOrdealRewardType.Fixed;
            if (jItemReward.TryGetProperty("type", out JsonElement jRewardType))
            {
                if (!Enum.TryParse(jRewardType.GetString(), true, out reward.Type))
                {
                    Logger.Error($"Failed to parse epitaph reward type. Failed.");
                    return null;
                }
            }

            reward.Rolls = 1;
            if (jItemReward.TryGetProperty("rolls", out JsonElement jRolls))
            {
                reward.Rolls = jRolls.GetUInt32();
            }

            if (reward.Type != SoulOrdealRewardType.Pool)
            {
                var itemId = jItemReward.GetProperty("item_id").GetUInt32();
                var amount = jItemReward.GetProperty("amount").GetUInt32();

                var isHidden = false;
                if (jItemReward.TryGetProperty("is_hidden", out JsonElement jIsHidden))
                {
                    isHidden = jIsHidden.GetBoolean();
                }

                double chance = 1.0;
                if (jItemReward.TryGetProperty("chance", out JsonElement jChance))
                {
                    chance = jChance.GetDouble();
                }

                reward.Items.Add((itemId, amount, reward.Type, isHidden, chance));
            }
            else
            {
                foreach (var jPoolItem in jItemReward.GetProperty("items").EnumerateArray())
                {
                    var itemId = jPoolItem.GetProperty("item_id").GetUInt32();
                    var amount = jPoolItem.GetProperty("amount").GetUInt32();

                    var type = SoulOrdealRewardType.Fixed;
                    if (jPoolItem.TryGetProperty("type", out JsonElement jPoolRewardType))
                    {
                        if (!Enum.TryParse(jPoolRewardType.GetString(), true, out type))
                        {
                            Logger.Error($"Failed to parse reward type for epitah pool item. Failed.");
                            return null;
                        }
                    }

                    var isHidden = false;
                    if (jPoolItem.TryGetProperty("is_hidden", out JsonElement jIsHidden))
                    {
                        isHidden = jIsHidden.GetBoolean();
                    }

                    double chance = 1.0;
                    if (jPoolItem.TryGetProperty("chance", out JsonElement jChance))
                    {
                        chance = jChance.GetDouble();
                    }

                    reward.Items.Add((itemId, amount, type, isHidden, chance));
                }
            }

            return reward;
        }
    }
}
