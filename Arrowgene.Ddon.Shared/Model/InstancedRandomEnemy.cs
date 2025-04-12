using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public class InstancedRandomEnemy : InstancedEnemy
    {
        protected List<(EnemyId, NamedParam)> Enemies;
        public Dictionary<EnemyId, DropsTable> DropTables;

        public InstancedRandomEnemy(List<EnemyId> enemyIds, Dictionary<EnemyId, DropsTable> dropTables)
        {
            Enemies = enemyIds.Select(x => (x, NamedParam.DEFAULT_NAMED_PARAM)).ToList();
            DropTables = dropTables;
        }

        public InstancedRandomEnemy(List<EnemyId> enemyIds, Dictionary<EnemyId, DropsTable> dropTables, ushort lv, uint exp, byte index)
        {
            // Custom Fields
            Enemies = enemyIds.Select(x => (x, NamedParam.DEFAULT_NAMED_PARAM)).ToList();
            DropTables = dropTables;

            var enemyParams = RollEnemyParams();

            Id = (uint)enemyParams.EnemyId;
            EnemyId = Id;
            Lv = lv;
            Experience = exp;
            Index = index;
            EnemyTargetTypesId = 4;
            Scale = 100;
            IsRequired = true;
            HmPresetNo = (ushort) ((EnemyId)EnemyId).GetHmPresetId();
            NamedEnemyParams = enemyParams.NamedParam;
        }

        public InstancedRandomEnemy(List<(EnemyId EnemyId, NamedParam NamedParam)> enemies, Dictionary<EnemyId, DropsTable> dropTables, ushort lv, uint exp, byte index)
        {
            // Custom Fields
            Enemies = enemies;
            DropTables = dropTables;

            var enemyParams = RollEnemyParams();

            Id = (uint)enemyParams.EnemyId;
            EnemyId = Id;
            Lv = lv;
            Experience = exp;
            Index = index;
            EnemyTargetTypesId = 4;
            Scale = 100;
            IsRequired = true;
            HmPresetNo = (ushort)((EnemyId)EnemyId).GetHmPresetId();
            NamedEnemyParams = enemyParams.NamedParam;
        }

        public override InstancedEnemy CreateNewInstance()
        {
            var enemyParams = RollEnemyParams();

            var enemy = base.CreateNewInstance();
            enemy.Id = (uint)enemyParams.EnemyId;
            enemy.EnemyId = (uint)enemyParams.EnemyId;
            enemy.HmPresetNo = (ushort)enemyParams.EnemyId.GetHmPresetId();
            enemy.NamedEnemyParams = enemyParams.NamedParam;
            enemy.DropsTable = DropTables[enemyParams.EnemyId];

            return enemy;
        }

        //Adds drop to all enemy drop tables in pool of random enemies
        public override InstancedEnemy AddDrop(ItemId itemId, uint minAmount, uint maxAmount, double chance, uint quality = 0, bool isHidden = false) {
            foreach(var (enemyId, _) in Enemies) {
                var table = DropTables[enemyId].Clone().AddDrop(itemId, minAmount, maxAmount, chance, quality, isHidden);
                DropTables[enemyId] = table;
            }
            return this;
        }

        private (EnemyId EnemyId, NamedParam NamedParam) RollEnemyParams()
        {
            return Enemies[Random.Shared.Next(0, Enemies.Count)];
        }
    }
}
