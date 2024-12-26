using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class BonusDungeonAssetDeserializer : IAssetDeserializer<BonusDungeonAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(BonusDungeonAssetDeserializer));

        public BonusDungeonAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            BonusDungeonAsset asset = new BonusDungeonAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var jCategory in document.RootElement.EnumerateArray())
            {
                var category = new BonusDungeonCategory()
                {
                    CategoryId = jCategory.GetProperty("category_id").GetUInt32(),
                    CategoryName = jCategory.GetProperty("name").GetString()
                };

                asset.DungeonCategories[category.CategoryId] = category;

                uint dungeonId = 0;
                foreach (var jDungeon in jCategory.GetProperty("dungeons").EnumerateArray())
                {
                    var dungeonInfo = new BonusDungeonInfo()
                    {
                        DungeonId = CreateDungeonId(category.CategoryId, dungeonId++),
                        EventName = jDungeon.GetProperty("name").GetString(),
                        StageId = jDungeon.GetProperty("stage_id").GetUInt32(),
                        StartingPos = jDungeon.GetProperty("starting_position").GetUInt32()
                    };
                    asset.DungeonInfo[dungeonInfo.DungeonId] = dungeonInfo;

                    foreach (var jItem in jDungeon.GetProperty("entry_fee").EnumerateArray())
                    {
                        dungeonInfo.EntryCostList.Add(new CDataStageDungeonItem()
                        {
                            ItemId = jItem.GetProperty("item_id").GetUInt32(),
                            Num = jItem.GetProperty("amount").GetUInt16()
                        });
                    }

                    category.DungeonInformation[dungeonInfo.DungeonId] = dungeonInfo;
                }
            }

            return asset;
        }

        private uint CreateDungeonId(uint categoryType, uint dungeonId)
        {
            return ((categoryType << 24) | dungeonId);
        }
    }
}


