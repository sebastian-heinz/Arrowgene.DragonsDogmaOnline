using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class EnemySpawnAssetDeserializer : IAssetDeserializer<EnemySpawnAsset>
    {

        // Consider this a macro that may be changed for balance
        // reasons. Each enemy drops PP equal to its experience points
        // divided by this number, unless an explicit value is
        // provided (called "PPDrop") in which case that value is used
        // instead.
        private static uint EXP_PER_PP = 7500;

        private static readonly ILogger Logger = LogProvider.Logger(typeof(EnemySpawnAssetDeserializer));

        private static readonly string[] ENEMY_HEADERS = {"StageId", "LayerNo", "GroupId", "SubGroupId", "EnemyId", "NamedEnemyParamsId", "RaidBossId", "Scale", "Lv", "HmPresetNo", "StartThinkTblNo", "RepopNum", "RepopCount", "EnemyTargetTypesId", "MontageFixNo", "SetType", "InfectionType", "IsBossGauge", "IsBossBGM", "IsManualSet", "IsAreaBoss", "IsBloodOrbEnemy", "BloodOrbs", "IsHighOrbEnemy", "HighOrbs", "Experience", "DropsTableId", "SpawnTime", "PPDrop"};
        private static readonly string[] DROPS_TABLE_HEADERS = {"ItemId", "ItemNum", "MaxItemNum", "Quality", "IsHidden", "DropChance"};

        private Dictionary<uint, NamedParam> namedParams;

        public EnemySpawnAssetDeserializer(Dictionary<uint, NamedParam> namedParams)
        {
            this.namedParams = namedParams;
        }

        public EnemySpawnAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            EnemySpawnAsset asset = new EnemySpawnAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            JsonElement schemasElement = document.RootElement.GetProperty("schemas");
            List<string> dropsTablesItemsSchema = schemasElement.GetProperty("dropsTables.items").EnumerateArray().Select(element => element.GetString()).ToList();
            Dictionary<string, int> dropsTableSchemaIndexes = findSchemaIndexes(DROPS_TABLE_HEADERS, dropsTablesItemsSchema);
            List<string> enemiesSchema = schemasElement.GetProperty("enemies").EnumerateArray().Select(element => element.GetString()).ToList();
            Dictionary<string, int> enemySchemaIndexes = findSchemaIndexes(ENEMY_HEADERS, enemiesSchema);


            JsonElement dropsTablesElement = document.RootElement.GetProperty("dropsTables");
            foreach (JsonElement dropsTableElement in dropsTablesElement.EnumerateArray())
            {
                uint id = dropsTableElement.GetProperty("id").GetUInt32();
                DropsTable dropsTable = asset.DropsTables.GetValueOrDefault(id) ?? new DropsTable();
                dropsTable.Id = id;
                dropsTable.Name = dropsTableElement.GetProperty("name").GetString();
                dropsTable.MdlType = dropsTableElement.GetProperty("mdlType").GetByte();
                foreach (JsonElement dropsTableItemsRow in dropsTableElement.GetProperty("items").EnumerateArray())
                {
                    List<JsonElement> row = dropsTableItemsRow.EnumerateArray().ToList();
                    GatheringItem gatheringItem = new GatheringItem();
                    gatheringItem.ItemId = (ItemId) row[dropsTableSchemaIndexes["ItemId"]].GetUInt32();
                    gatheringItem.ItemNum = row[dropsTableSchemaIndexes["ItemNum"]].GetUInt32();
                    gatheringItem.MaxItemNum = row[dropsTableSchemaIndexes["MaxItemNum"]].GetUInt32();
                    gatheringItem.Quality = row[dropsTableSchemaIndexes["Quality"]].GetUInt32();
                    gatheringItem.IsHidden = row[dropsTableSchemaIndexes["IsHidden"]].GetBoolean();

                    // Optional for compatibility with older formats
                    if(dropsTableSchemaIndexes.ContainsKey("DropChance"))
                    {
                        gatheringItem.DropChance = row[dropsTableSchemaIndexes["DropChance"]].GetDouble();
                    }
                    else
                    {
                        gatheringItem.DropChance = 1;
                    }

                    dropsTable.Items.Add(gatheringItem);
                }

                asset.DropsTables[id] = dropsTable;
            }

            JsonElement enemiesElement = document.RootElement.GetProperty("enemies");
            foreach (JsonElement enemyRow in enemiesElement.EnumerateArray())
            {
                List<JsonElement> row = enemyRow.EnumerateArray().ToList();
                StageLayoutId layoutId = new StageLayoutId(
                    row[enemySchemaIndexes["StageId"]].GetUInt32(),
                    row[enemySchemaIndexes["LayerNo"]].GetByte(),
                    row[enemySchemaIndexes["GroupId"]].GetUInt32()
                );
                byte subGroupId = row[enemySchemaIndexes["SubGroupId"]].GetByte();
                List<Enemy> enemies = asset.Enemies.GetValueOrDefault(layoutId) ?? new List<Enemy>();
                Enemy enemy = new Enemy()
                {
                    EnemyId = ParseHexUInt(row[enemySchemaIndexes["EnemyId"]].GetString()),
                    NamedEnemyParams = this.namedParams.GetValueOrDefault(row[enemySchemaIndexes["NamedEnemyParamsId"]].GetUInt32(), NamedParam.DEFAULT_NAMED_PARAM),
                    RaidBossId = row[enemySchemaIndexes["RaidBossId"]].GetUInt32(),
                    Scale = row[enemySchemaIndexes["Scale"]].GetUInt16(),
                    Lv = row[enemySchemaIndexes["Lv"]].GetUInt16(),
                    HmPresetNo = row[enemySchemaIndexes["HmPresetNo"]].GetUInt16(),
                    StartThinkTblNo = row[enemySchemaIndexes["StartThinkTblNo"]].GetByte(),
                    RepopNum = row[enemySchemaIndexes["RepopNum"]].GetByte(),
                    RepopCount = row[enemySchemaIndexes["RepopCount"]].GetByte(),
                    EnemyTargetTypesId = row[enemySchemaIndexes["EnemyTargetTypesId"]].GetByte(),
                    MontageFixNo = row[enemySchemaIndexes["MontageFixNo"]].GetByte(),
                    SetType = row[enemySchemaIndexes["SetType"]].GetByte(),
                    InfectionType = row[enemySchemaIndexes["InfectionType"]].GetByte(),
                    IsBossGauge = row[enemySchemaIndexes["IsBossGauge"]].GetBoolean(),
                    IsBossBGM = row[enemySchemaIndexes["IsBossBGM"]].GetBoolean(),
                    IsManualSet = row[enemySchemaIndexes["IsManualSet"]].GetBoolean(),
                    IsAreaBoss = row[enemySchemaIndexes["IsAreaBoss"]].GetBoolean(),
                    BloodOrbs = row[enemySchemaIndexes["BloodOrbs"]].GetUInt32(),
                    HighOrbs = row[enemySchemaIndexes["HighOrbs"]].GetUInt32(),
                    Experience = row[enemySchemaIndexes["Experience"]].GetUInt32(),

                    Subgroup = subGroupId,
                };

                // Fallback for old style schema
                enemy.IsBloodOrbEnemy = enemy.BloodOrbs > 0;
                if (enemySchemaIndexes.ContainsKey("IsBloodOrbEnemy"))
                {
                    enemy.IsBloodOrbEnemy = row[enemySchemaIndexes["IsBloodOrbEnemy"]].GetBoolean();
                }

                // Fallback for old style schema
                enemy.IsHighOrbEnemy = enemy.HighOrbs > 0;
                if (enemySchemaIndexes.ContainsKey("IsHighOrbEnemy"))
                {
                    enemy.IsHighOrbEnemy = row[enemySchemaIndexes["IsHighOrbEnemy"]].GetBoolean();
                }

                // checking if the file has spawntime, if yes we convert the time and pass it along to enemy.cs
                if (enemySchemaIndexes.ContainsKey("SpawnTime"))
                {
                    string SpawnTimeGet = row[enemySchemaIndexes["SpawnTime"]].GetString();
                    var (start, end) = AssetCommonDeserializer.ConvertTimeToMilliseconds(SpawnTimeGet);
                    enemy.SpawnTimeStart = start;
                    enemy.SpawnTimeEnd = end;
                }
                else
                {
                    // if no, we define it to the "allday" spawn time range and pass this along to the enemy.cs instead.
                    var (start, end) = AssetCommonDeserializer.ConvertTimeToMilliseconds("00:00,23:59");
                    enemy.SpawnTimeStart = start;
                    enemy.SpawnTimeEnd = end;
                }

                // check if the file has PPDrop.
                if(enemySchemaIndexes.ContainsKey("PPDrop"))
                {
                    // If yes, get it as a uint32.
                    enemy.PPDrop = row[enemySchemaIndexes["PPDrop"]].GetUInt32();
                }
                else
                {
                    // If no, set the value to its experience / EXP_PER_PP.
                    enemy.PPDrop = enemy.Experience / EXP_PER_PP;
                }
                
                int dropsTableId = row[enemySchemaIndexes["DropsTableId"]].GetInt32();
                if(dropsTableId >= 0 && asset.DropsTables.ContainsKey((uint)dropsTableId))
                {
                    enemy.DropsTable = asset.DropsTables[(uint) dropsTableId];
                }
                enemies.Add(enemy);
                asset.Enemies[layoutId] = enemies;
            }

            return asset;
        }

        private Dictionary<string, int> findSchemaIndexes(string[] reference, List<string> schema)
        {
            Dictionary<string, int> schemaIndexes = new Dictionary<string, int>();
            foreach (string header in reference)
            {
                int index = schema.IndexOf(header);
                if (index != -1)
                {
                    schemaIndexes.Add(header, index);
                }
            }
            return schemaIndexes;
        }

        private uint ParseHexUInt(string str)
        {
            str = str.TrimStart('0', 'x');
            return uint.Parse(str, NumberStyles.HexNumber, null);
        }
    }
}
