using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class LimitBreakAssetReader : IAssetDeserializer<LimitBreakAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(LimitBreakAssetReader));

        public LimitBreakAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            LimitBreakAsset asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            asset.ListTitle = document.RootElement.GetProperty("title").ToString();

            var categories = new List<string>()
            {
                "weapon_lottery", "armor_lottery"
            };

            foreach (var categoryName in categories)
            {
                var jLotteryCategory = document.RootElement.GetProperty(categoryName);

                LimitBreakCategory lotteryCategory = new();
                foreach (var jListing in jLotteryCategory.GetProperty("shop_listings").EnumerateArray())
                {
                    lotteryCategory.ShopListings.Add(jListing.GetByte());
                }

                foreach (var jPremiumCurrency in jLotteryCategory.GetProperty("premium_payments").EnumerateArray())
                {
                    if (!Enum.TryParse(jPremiumCurrency.GetString(), true, out WalletType walletType))
                    {
                        continue;
                    }
                    lotteryCategory.PremiumCurrencies.Add(walletType);
                }

                foreach (var jPaymentOption in jLotteryCategory.GetProperty("payment_options").EnumerateArray())
                {
                    if (!Enum.TryParse(jPaymentOption.GetProperty("type").GetString(), true, out WalletType walletType))
                    {
                        continue;
                    }

                    string optionLabel = jPaymentOption.GetProperty("name").GetString();
                    uint costAmount = jPaymentOption.GetProperty("amount").GetUInt32();

                    lotteryCategory.PaymentOptions.Add((walletType, optionLabel, costAmount));
                }

                foreach (var jStatList in jLotteryCategory.GetProperty("stats").EnumerateArray())
                {
                    var stat = new LimitStatLottery()
                    {
                        MinGreatSuccessIndex = jStatList.GetProperty("min_great_success_index").GetUInt32()
                    };

                    foreach (var jId in jStatList.GetProperty("ids").EnumerateArray())
                    {
                        stat.Rolls.Add(jId.GetUInt16());
                    }
                    lotteryCategory.StatLottery.Add(stat);
                }

                asset.Categories.Add(lotteryCategory);
            }

            return asset;
        }
    }
}
