using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Data;
using System;
using System.IO;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class GachaAssetDeserializer : IAssetDeserializer<GachaAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(GachaAssetDeserializer));

        public GachaAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            GachaAsset asset = new GachaAsset();

            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var jLootBox in document.RootElement.GetProperty("boxes").EnumerateArray())
            {
                var gachaInfo = new CDataGachaInfo()
                {
                    Id = jLootBox.GetProperty("gacha_id").GetUInt32(),
                    Begin = jLootBox.GetProperty("start_time").GetInt64(),
                    End = jLootBox.GetProperty("end_time").GetInt64(),
                    Name = jLootBox.GetProperty("name").GetString(),
                    Description = jLootBox.GetProperty("description").GetString(),
                    Detail = jLootBox.GetProperty("detail").GetString(),
                    WeightDispType = jLootBox.GetProperty("weight_display_type").GetByte(),
                    WeightDispTitle = jLootBox.GetProperty("weight_display_title").GetString(),
                    WeightDispText = jLootBox.GetProperty("weight_display_text").GetString(),
                    ListAddr = jLootBox.GetProperty("list_address").GetString(),
                    ImageAddr = jLootBox.GetProperty("image_address").GetString(),
                };


                var drawGroupInfo = new CDataGachaDrawGroupInfo();
                foreach (var jSettlementInfo in jLootBox.GetProperty("draw_groups").EnumerateArray())
                {
                    WalletType walletType;
                    if (!Enum.TryParse(jSettlementInfo.GetProperty("wallet_type").GetString(), true, out walletType))
                    {
                        Logger.Error($"Failed to parse WalletType for {path}. Skipping.");
                        break;    
                    }

                    if (walletType != WalletType.GoldenGemstones && walletType != WalletType.SilverTickets)
                    {
                        Logger.Error($"The currency '{walletType}' is not a valid loot box currency. Skipping.");
                        break;
                    }

                    var settlementInfo = new CDataGachaSettlementInfo()
                    {
                        Id = (walletType == WalletType.GoldenGemstones) ? 1u : 2u,
                        DrawGroupId = jSettlementInfo.GetProperty("group_id").GetUInt32(),
                        Price = jSettlementInfo.GetProperty("price").GetUInt32(),
                        BasePrice = jSettlementInfo.GetProperty("base_price").GetUInt32(),
                        PurchaseNum = jSettlementInfo.GetProperty("purchase_num").GetUInt32(),
                        PurchaseMaxNum = jSettlementInfo.GetProperty("purchase_max_num").GetUInt32(),
                        SpecialPriceNum = jSettlementInfo.GetProperty("special_price_num").GetUInt32(),
                        SpecialPriceMaxNum = jSettlementInfo.GetProperty("special_price_max_num").GetUInt32(),
                        Unk1 = jSettlementInfo.GetProperty("unk1").GetUInt32(),
                    };

                    drawGroupInfo.GachaSettlementList.Add(settlementInfo);
                }

                var drawInfo = new CDataGachaDrawInfo()
                {
                    Num = 1,
                    IsBonus = false
                };

                foreach (var jItem in jLootBox.GetProperty("draw_list").EnumerateArray())
                {
                    drawInfo.GachaItemInfo.Add(new CDataGachaItemInfo()
                    {
                        ItemId = jItem.GetProperty("item_id").GetUInt32(),
                        ItemNum = jItem.GetProperty("amount").GetUInt32(),
                        Rank = jItem.GetProperty("rank").GetUInt32(),
                        Effect = jItem.GetProperty("effect").GetUInt32(),
                        Probability = jItem.GetProperty("chance").GetDouble()
                    });
                }
                drawGroupInfo.GachaDrawList.Add(drawInfo);

                gachaInfo.DrawGroups.Add(drawGroupInfo);

                asset.GachaInfoList[gachaInfo.Id] = gachaInfo;
            }

            return asset;
        }
    }
}
