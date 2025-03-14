using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IMonsterSpotInfo
    {
        public abstract StageLayoutId StageLayoutId { get; }
        public virtual byte SubGroupId { get; } = 0;
        public abstract QuestAreaId AreaId { get; }
        public virtual bool CautionPlayer { get; } = true;
        public abstract uint RequiredAreaRank { get; }
        protected Dictionary<uint, List<InstancedEnemy>> Enemies { get; set; }

        public IMonsterSpotInfo()
        {
            Enemies = new Dictionary<uint, List<InstancedEnemy>>();
        }

        public (StageLayoutId StageLayoutId, byte SubGroupId) GetLocation()
        {
            return (StageLayoutId, SubGroupId);
        }

        protected void AddEnemy(InstancedEnemy enemy, uint enemyGroupId = 0)
        {
            enemy.RequiredAreaRank = RequiredAreaRank;
            enemy.EnemyTargetTypesId = 1;

            if (!Enemies.ContainsKey(enemyGroupId))
            {
                Enemies[enemyGroupId] = new List<InstancedEnemy>();
            }

            Enemies[enemyGroupId].Add(enemy);
        }

        protected void AddEnemies(List<InstancedEnemy> enemies, uint enemyGroupId = 0)
        {
            foreach (var enemy in enemies)
            {
                AddEnemy(enemy, enemyGroupId);
            }
        }

        public abstract void Initialize();

        public virtual ReadOnlyCollection<InstancedEnemy> GetInstanceEnemies()
        {
            return Enemies.ContainsKey(0) ? Enemies[0].AsReadOnly() : new List<InstancedEnemy>().AsReadOnly();
        }
    }
}
