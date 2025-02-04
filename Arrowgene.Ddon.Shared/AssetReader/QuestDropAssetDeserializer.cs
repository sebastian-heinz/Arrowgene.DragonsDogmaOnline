using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Logging;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class QuestDropAssetDeserializer : IAssetDeserializer<QuestDropItemAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(QuestDropItemAsset));

        public QuestDropItemAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            QuestDropItemAsset asset = new QuestDropItemAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            JsonElement dropsTablesElement = document.RootElement.GetProperty("dropsTables");

            foreach (JsonElement dropsTableElement in dropsTablesElement.EnumerateArray())
            {
                // ignore flag is optional for non implemented default / monsters with WIP tables
                if (dropsTableElement.TryGetProperty("ignore", out JsonElement ignore))
                {
                    if (ignore.GetBoolean())
                    {
                        continue;
                    }
                }

                DropsTable table = new();

                uint dropTableId = dropsTableElement.GetProperty("id").GetUInt32();

                table.Id = dropTableId;
                table.Name = dropsTableElement.GetProperty("name").GetString();
                table.MdlType = dropsTableElement.GetProperty("mdlType").GetByte();

                // Set default for max level
                ushort maxLevel = 0;

                ushort minLevel = dropsTableElement.GetProperty("min_level").GetUInt16();

                if(dropsTableElement.TryGetProperty("max_level", out JsonElement level))
                {
                    maxLevel = level.GetUInt16();
                }

                // Check now if the levels are correctly set
                if(maxLevel != 0 && maxLevel < minLevel)
                {
                    throw new Exception($"min_level and max_level are both defined, but min_level value is higher at id {dropTableId}");
                }

                foreach (JsonElement dropsTableItemsRow in dropsTableElement.GetProperty("items").EnumerateArray())
                {
                    List<JsonElement> row = dropsTableItemsRow.EnumerateArray().ToList();

                    GatheringItem gatheringItem = new()
                    {
                        ItemId = (ItemId) row[(int)QuestEnemyDropHeaders.ItemId].GetUInt32(),
                        ItemNum = row[(int)QuestEnemyDropHeaders.ItemNum].GetUInt32(),
                        MaxItemNum = row[(int)QuestEnemyDropHeaders.MaxItemNum].GetUInt32(),
                        Quality = row[(int)QuestEnemyDropHeaders.Quality].GetUInt32(),
                        IsHidden = row[(int)QuestEnemyDropHeaders.IsHidden].GetBoolean(),
                        DropChance = row[(int)QuestEnemyDropHeaders.DropChance].GetDouble()
                    };


                    table.Items.Add(gatheringItem);
                }


                List<JsonElement> enemyIds = dropsTableElement.GetProperty("enemy_id").EnumerateArray().ToList();

                foreach (var enemy in enemyIds)
                {

                    var enemyId = Convert.ToUInt32(enemy.GetString(), 16);


                    if (maxLevel > 0)
                    {
                        // Add the drop table with max level defined.
                        asset.AddDropTable(enemyId, minLevel, maxLevel, table);
                    }
                    else
                    {
                        // Add drop table only with min level.
                        asset.AddDropTable(enemyId, minLevel, table);
                    }
                }
            }

            return asset;
        }

        public enum QuestEnemyDropHeaders
        {
            ItemId = 0,
            ItemNum = 1,
            MaxItemNum= 2,
            Quality = 3,
            IsHidden = 4,
            DropChance = 5
        }
    }
}
