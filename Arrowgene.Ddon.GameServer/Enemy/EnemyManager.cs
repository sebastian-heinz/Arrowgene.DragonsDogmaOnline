using System;
using System.Collections.Generic;
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

        private readonly Dictionary<Tuple<StageId, byte>, List<EnemySpawn>> _spawns;
        private readonly AssetRepository _assetRepository;
        private readonly IDatabase _database;

        public EnemyManager(AssetRepository assetRepository, IDatabase database)
        {
            _assetRepository = assetRepository;
            _database = database;
            _spawns = new Dictionary<Tuple<StageId, byte>, List<EnemySpawn>>();

            Load();
            _assetRepository.AssetChanged += AssetRepositoryOnAssetChanged;
        }

        public List<EnemySpawn> GetSpawns(CStageLayoutId stageLayoutId, byte subGroupId)
        {
            return GetSpawns(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<EnemySpawn> GetSpawns(StageId stageId, byte subGroupId)
        {
            // TODO check time of day, area rank, and other parameters
            return _spawns.GetValueOrDefault(new Tuple<StageId, byte>(stageId, subGroupId), new List<EnemySpawn>());
        }

        public void Load()
        {
            _spawns.Clear();
            foreach (EnemySpawn spawn in _assetRepository.EnemySpawns)
            {
                Tuple<StageId, byte> tuple = new Tuple<StageId, byte>(spawn.StageId, spawn.SubGroupId);
                List<EnemySpawn> spawnsForTuple = _spawns.GetValueOrDefault(tuple, new List<EnemySpawn>());
                spawnsForTuple.Add(spawn);
                _spawns[tuple] =  spawnsForTuple; // Adds or updates if the tuple is already present
            }
        }
        
        private void AssetRepositoryOnAssetChanged(object sender, AssetChangedEventArgs e)
        {
            if (e.Key == AssetRepository.EnemySpawnsKey)
            {
                Load();
            }
        }

    }
}
