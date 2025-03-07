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

        public List<InstancedEnemy> CreateNewInstance(ushort processNo = 0, ushort blockNo = 0)
        {
            List<InstancedEnemy> results = new List<InstancedEnemy>();
            foreach (var enemy in Enemies)
            {
                var newEnemy = enemy.CreateNewInstance();
                newEnemy.QuestProcessInfo.ProcessNo = processNo;
                newEnemy.QuestProcessInfo.BlockNo = blockNo;
                results.Add(newEnemy);
            }
            return results;
        }
    }
}
