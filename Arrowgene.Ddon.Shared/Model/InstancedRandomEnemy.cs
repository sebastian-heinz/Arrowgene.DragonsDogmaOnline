using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class InstancedRandomEnemy : InstancedEnemy
    {
        protected List<EnemyId> EnemyIds;
        public Dictionary<EnemyId, DropsTable> DropTables;

        public InstancedRandomEnemy(List<EnemyId> enemyIds, Dictionary<EnemyId, DropsTable> dropTables)
        {
            EnemyIds = enemyIds;
            DropTables = dropTables;
        }

        public InstancedRandomEnemy(List<EnemyId> enemyIds, Dictionary<EnemyId, DropsTable> dropTables, ushort lv, uint exp, byte index)
        {
            // Custom Fields
            EnemyIds = enemyIds;
            DropTables = dropTables;

            Id = (uint) RollEnemyId();
            EnemyId = Id;
            Lv = lv;
            Experience = exp;
            Index = index;
            EnemyTargetTypesId = 4;
            Scale = 100;
            IsRequired = true;
            HmPresetNo = (ushort) ((EnemyId)EnemyId).GetHmPresetId();
        }

        public override InstancedEnemy CreateNewInstance()
        {
            var enemyId = RollEnemyId();

            var enemy = base.CreateNewInstance();
            enemy.Id = (uint)enemyId;
            enemy.EnemyId = (uint)enemyId;
            enemy.HmPresetNo = (ushort)enemyId.GetHmPresetId();

            enemy.DropsTable = DropTables[enemyId];

            return enemy;
        }

        private EnemyId RollEnemyId()
        {
            return EnemyIds[Random.Shared.Next(0, EnemyIds.Count)];
        }
    }
}
