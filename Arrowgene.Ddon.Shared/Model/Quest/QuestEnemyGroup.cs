using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestEnemyGroup
    {
        public uint GroupId { get; set; }
        public byte SubGroupId { get; set; }
        public StageId StageId { get; set; }
        public uint StartingIndex { get; set; }
        public List<InstancedEnemy> Enemies { get; set; }
        public QuestEnemyPlacementType PlacementType { get; set; }

        public QuestEnemyGroup()
        {
            Enemies = new List<InstancedEnemy>();
            StageId = StageId.Invalid;
            PlacementType = QuestEnemyPlacementType.Automatic;
        }

        public List<InstancedEnemy> CreateNewInstance()
        {
            List<InstancedEnemy> results = new List<InstancedEnemy>();

            for (var i = 0; i < Enemies.Count; i++)
            {
                var enemy = Enemies[i];
                results.Add(new InstancedEnemy(enemy)
                {
                    Index = (PlacementType == QuestEnemyPlacementType.Automatic) ? (byte)(i + StartingIndex) : enemy.Index,
                    IsQuestControlled = true,
                    StageId = StageId
                });
            }
            return results;
        }
    }
}
