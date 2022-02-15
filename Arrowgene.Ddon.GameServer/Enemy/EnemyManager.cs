using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Enemy
{
    public class EnemyManager
    {
        private readonly Dictionary<StageId, Dictionary<byte, List<EnemySpawn>>> _spawns;

        public EnemyManager()
        {
            _spawns = new Dictionary<StageId, Dictionary<byte, List<EnemySpawn>>>();
        }

        public List<EnemySpawn> GetSpawns(CStageLayoutID stageLayoutId, byte subGroupId)
        {
            return GetSpawns(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<EnemySpawn> GetSpawns(StageId stageId, byte subGroupId)
        {
            // TODO populate _spawns
            EnemySpawn es = new EnemySpawn();
            es.Enemy.EnemyId = 0x010100;
            es.Enemy.NamedEnemyParamsId = 0x8FA;
            es.Enemy.Scale = 100;
            es.Enemy.Lv = 66;
            es.Enemy.EnemyTargetTypesId = 1;
            return new List<EnemySpawn> {es};

            if (!_spawns.ContainsKey(stageId))
            {
                return new List<EnemySpawn>();
            }

            Dictionary<byte, List<EnemySpawn>> stageSpawns = _spawns[stageId];
            if (!stageSpawns.ContainsKey(subGroupId))
            {
                return new List<EnemySpawn>();
            }

            // TODO check time of day
            
            return new List<EnemySpawn>(stageSpawns[subGroupId]);
        }

        public void Load(IDatabase database)
        {
        }

        public void Save(IDatabase database)
        {
        }
    }
}
