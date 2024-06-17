using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestEnemyGroup
    {
        public uint SubGroupId { get; set; }
        public StageId StageId { get; set; }
        public uint StartingIndex { get; set; }
        public List<Enemy> Enemies { get; set; }

        public QuestEnemyGroup()
        {
            Enemies = new List<Enemy>();
            StageId = StageId.Invalid;
        }

        public List<InstancedEnemy> AsInstancedEnemies()
        {
            List<InstancedEnemy> results = new List<InstancedEnemy>();
            for (var i = 0; i < Enemies.Count; i++)
            {
                var enemy = Enemies[i];
                results.Add(new InstancedEnemy(enemy)
                {
                    Index = (byte)(i + StartingIndex)
                });
            }
            return results;
        }
    }
}
