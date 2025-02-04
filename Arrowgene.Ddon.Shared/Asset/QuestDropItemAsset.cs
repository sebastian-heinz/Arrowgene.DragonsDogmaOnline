using Arrowgene.Ddon.Shared.AssetReader;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class QuestDropItemAsset
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(QuestDropItemAsset));

        public QuestDropItemAsset()
        {

            QuestDropsTables = new();
            EnemyLevelMap = new();
        }
        // <dropTableId, DropTable>>
        private Dictionary<uint, DropsTable> QuestDropsTables { get; set; }

        // <enemyId, <enemyLevel, DropTableId>>
        private Dictionary<uint, SortedList<ushort, uint>> EnemyLevelMap { get; set; }

        // This is to be used within the quest drop table serializer for adding the default drop tables
        public void AddDropTable(uint enemyId, ushort levelMin, ushort levelMax, DropsTable dropTable)
        {
            if (!EnemyLevelMap.ContainsKey(enemyId))
            {
                EnemyLevelMap[enemyId] = new SortedList<ushort, uint>();
            }

            // Create a mapping of the enemy id level values to the correct drop table id.

            for (ushort i = levelMin; i <= levelMax; i++)
            {
                EnemyLevelMap[enemyId].Add(i, dropTable.Id);
            }

            // Add the drop table with the drop table id to the main DropsTable
            QuestDropsTables[dropTable.Id] = dropTable;
        }

        public void AddDropTable(uint enemyId, ushort levelMin, DropsTable dropTable)
        {
            if (!EnemyLevelMap.ContainsKey(enemyId))
            {
                EnemyLevelMap[enemyId] = new SortedList<ushort, uint>();
            }

            // Create a mapping of the enemy id level values to the correct drop table id.

            EnemyLevelMap[enemyId].Add(levelMin, dropTable.Id);

            // Add the drop table with the drop table id to the main DropsTable
            QuestDropsTables[dropTable.Id] = dropTable;
        }

        public DropsTable GetDropTable(uint enemyId, ushort enemyLevel)
        {
            if (EnemyLevelMap.ContainsKey(enemyId))
            {
                if (EnemyLevelMap[enemyId].ContainsKey(enemyLevel))
                {
                    return QuestDropsTables[EnemyLevelMap[enemyId][enemyLevel]];
                }

                // if we are here then the LevelMap doesn't contain an entry for this level.
                // Check the enemy level against the largest level
                ushort largestLevel = EnemyLevelMap[enemyId].Last().Key;

                if (largestLevel <= enemyLevel)
                {
                    return QuestDropsTables[EnemyLevelMap[enemyId][largestLevel]];
                }
            }

            // Nothing found if we are down here
            return new();
        }
    }
}
