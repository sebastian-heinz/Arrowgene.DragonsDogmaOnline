using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestEnemyGroup
    {
        public uint GroupId { get; set; }
        public byte SubGroupId { get; set; }
        public StageLayoutId StageLayoutId { get; set; }
        public uint StartingIndex { get; set; }
        public List<InstancedEnemy> Enemies { get; set; }
        public QuestEnemyPlacementType PlacementType { get; set; }
        public QuestTargetType TargetType { get; set; }

        public QuestEnemyGroup()
        {
            Enemies = new List<InstancedEnemy>();
            StageLayoutId = StageLayoutId.Invalid;
            PlacementType = QuestEnemyPlacementType.Automatic;
            TargetType = QuestTargetType.EnemyForOrder;
        }

        public List<InstancedEnemy> CreateNewInstance()
        {
            List<InstancedEnemy> results = new List<InstancedEnemy>();
            for (var i = 0; i < Enemies.Count; i++)
            {
                var enemy = Enemies[i];
                var instancedEnemy = enemy.CreateNewInstance();
                instancedEnemy.Index = (PlacementType == QuestEnemyPlacementType.Automatic) ? (byte)(i + StartingIndex) : enemy.Index;
                instancedEnemy.IsQuestControlled = true;
                instancedEnemy.StageId = StageLayoutId;
                results.Add(instancedEnemy);
            }
            return results;
        }
    }
}
