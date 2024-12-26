using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using System;
using static Arrowgene.Ddon.Shared.Csv.GmdCsv;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class SpecialShopDeserializer : IAssetDeserializer<SpecialShopAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SpecialShopDeserializer));

        public SpecialShopAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            SpecialShopAsset asset = new SpecialShopAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            uint categoryId = 0;
            var jShops = document.RootElement.GetProperty("shops").EnumerateArray();
            foreach (var jShop in jShops)
            {
                if (!Enum.TryParse(jShop.GetProperty("shop_type").GetString(), true, out ShopType shopType))
                {
                    Logger.Error($"The value for the key 'shop_type' is not a valid value.");
                    return null;
                }

                if (asset.SpecialShops.ContainsKey(shopType))
                {
                    Logger.Error($"The shop '{shopType}' is defined multiple times.");
                    return null;
                }

                asset.SpecialShops[shopType] = new List<ShopCategory>();
                foreach (var jCategory in jShop.GetProperty("categories").EnumerateArray())
                {
                    ShopCategory shopCategory = new ShopCategory()
                    {
                        Label = jCategory.GetProperty("label").GetString(),
                        Id = categoryId++,
                    };

                    ushort appraisialId = 0;
                    foreach (var jAppraisal in jCategory.GetProperty("appraisals").EnumerateArray())
                    {
                        AppraisalItem item = new AppraisalItem()
                        {
                            AppraisalId = CreateAppraisalId(shopType, shopCategory.Id, appraisialId++),
                            Label = jAppraisal.GetProperty("label").GetString()
                        };

                        foreach (var jBaseItem in jAppraisal.GetProperty("base_items").EnumerateArray())
                        {
                            AppraisalBaseItem baseItem = new AppraisalBaseItem()
                            {
                                ItemId = jBaseItem.GetProperty("item_id").GetUInt32(),
                                Amount = jBaseItem.GetProperty("amount").GetUInt32(),
                                Name = jBaseItem.GetProperty("name").GetString(),
                            };
                            item.BaseItems.Add(baseItem);
                        }

                        foreach (var jLotItem in jAppraisal.GetProperty("pool").EnumerateArray())
                        {
                            AppraisalLotteryItem lotItem = new AppraisalLotteryItem()
                            {
                                ItemId = jLotItem.GetProperty("item_id").GetUInt32(),
                                Name = jLotItem.GetProperty("name").GetString(),
                                Amount = jLotItem.GetProperty("amount").GetUInt32(),
                            };

                            if (jLotItem.TryGetProperty("crests", out JsonElement jCrests))
                            {
                                if (!ParseAppraisalCrests(jCrests, lotItem.Crests))
                                {
                                    return null;
                                }
                            }

                            item.LootPool.Add(lotItem);
                        }

                        asset.AppraisalItems[item.AppraisalId] = item;

                        shopCategory.Items.Add(item);
                    }

                    asset.SpecialShops[shopType].Add(shopCategory);
                    asset.ShopCategories[shopCategory.Id] = shopCategory;
                }
            }

            return asset;
        }

        private bool ParseAppraisalCrests(JsonElement jCrests, List<AppraisalCrest> results)
        {
            foreach (var crest in jCrests.EnumerateArray())
            {
                if (!Enum.TryParse(crest.GetProperty("type").GetString(), true, out AppraisalCrestType crestType))
                {
                    var invalidType = crest.GetProperty("type").GetString();
                    Logger.Error($"Invalid crest type '{invalidType}'. Unable to parse.");
                    return false;
                }

                uint crestId = 0;
                if (crest.TryGetProperty("crest_id", out JsonElement jCrestId))
                {
                    crestId = jCrestId.GetUInt32();
                }

                ushort amount = 0;
                if (crest.TryGetProperty("amount", out JsonElement jAmount))
                {
                    amount = jAmount.GetUInt16();
                }

                JobId jobId = JobId.None;
                if (crest.TryGetProperty("job_id", out JsonElement jJobId))
                {
                    if (!Enum.TryParse(jJobId.GetString(), true, out JobId pjobId))
                    {
                        var invalidType = crest.GetProperty("job_id").GetString();
                        Logger.Error($"Invalid JobId '{invalidType}'. Unable to parse.");
                        return false;
                    }
                    jobId = pjobId;
                }

                List<uint> crestLottery = new List<uint>();
                if (crest.TryGetProperty("values", out JsonElement jValues))
                {
                    foreach (var jValue in jValues.EnumerateArray())
                    {
                        crestLottery.Add(jValue.GetUInt32());
                    }
                }

                results.Add(new AppraisalCrest()
                {
                    CrestType = crestType,
                    CrestId = crestId,
                    Amount = amount,
                    JobId = jobId,
                    CrestLottery = crestLottery
                });
            }

            return true;
        }

        private uint CreateAppraisalId(ShopType shopType, uint categoryId,  ushort appraisalId)
        {
            byte bShopType = (byte) shopType;
            byte bCatId = (byte) categoryId;
            return (uint)((bShopType << 24) | (bCatId << 16) | appraisalId);
        }
    }
}

