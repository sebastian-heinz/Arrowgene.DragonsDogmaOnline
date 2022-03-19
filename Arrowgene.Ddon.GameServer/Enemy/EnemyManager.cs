using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Enemy
{
    public class EnemyManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EnemyManager));

        private readonly Dictionary<StageId, Dictionary<byte, List<EnemySpawn>>> _spawns;
        private readonly AssetRepository _assetRepository;
        private readonly IDatabase _database;        

        public EnemyManager(AssetRepository assetRepository, IDatabase database)
        {
            _assetRepository = assetRepository;
            _database = database;
            _spawns = new Dictionary<StageId, Dictionary<byte, List<EnemySpawn>>>();

            Load();
            _assetRepository.UpdatedEnemySpawnsEvent += (sender, e) => Load();
        }

        public List<EnemySpawn> GetSpawns(CStageLayoutId stageLayoutId, byte subGroupId)
        {
            return GetSpawns(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<EnemySpawn> GetSpawns(StageId stageId, byte subGroupId)
        {
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

        public void Load()
        {
            _spawns.Clear();
            foreach (EnemySpawn spawn in _assetRepository.EnemySpawns)
            {
                Dictionary<byte, List<EnemySpawn>> stageSpawns;
                if (_spawns.ContainsKey(spawn.StageId))
                {
                    stageSpawns = _spawns[spawn.StageId];
                }
                else
                {
                    stageSpawns = new Dictionary<byte, List<EnemySpawn>>();
                    _spawns.Add(spawn.StageId, stageSpawns);
                }

                List<EnemySpawn> spawns;
                if (stageSpawns.ContainsKey(spawn.SubGroupId))
                {
                    spawns = stageSpawns[spawn.SubGroupId];
                }
                else
                {
                    spawns = new List<EnemySpawn>();
                    stageSpawns.Add(spawn.SubGroupId, spawns);
                }

                spawns.Add(spawn);
            }
        }
    }
}
