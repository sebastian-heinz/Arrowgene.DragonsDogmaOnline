using System.Collections.Generic;
using System.IO;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Enemy.Csv;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Enemy
{
    public class EnemyManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EnemyManager));

        private readonly EnemySpawnCsvReader enemySpawnCsvReader;
        private Dictionary<StageId, Dictionary<byte, List<EnemySpawn>>> _spawns;

        public EnemyManager(string enemySetCsvFile)
        {
            this.enemySpawnCsvReader = new EnemySpawnCsvReader();
            this.enemySpawnCsvReader.AllowLF = true;

            this._spawns = new Dictionary<StageId, Dictionary<byte, List<EnemySpawn>>>();

            if(enemySetCsvFile != null) {
                // Listen for changes in the csv file and update the spawns
                FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(enemySetCsvFile), Path.GetFileName(enemySetCsvFile));
                watcher.Changed += (object sender, FileSystemEventArgs e) => this.LoadFromFile(e.FullPath);
                watcher.EnableRaisingEvents = true;

                // First time load
                this.LoadFromFile(enemySetCsvFile);
            }
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
            //return new List<EnemySpawn> {es};

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

        private void LoadFromFile(string file) {
            Logger.Debug($"Loading enemy sets from file {file}");

            List<EnemySpawn> enemySets = this.enemySpawnCsvReader.Read(file);

            this._spawns.Clear();
            foreach (EnemySpawn enemySet in enemySets)
            {
                if(!this._spawns.ContainsKey(enemySet.StageId))
                    this._spawns.Add(enemySet.StageId, new Dictionary<byte, List<EnemySpawn>>());

                this._spawns.TryGetValue(enemySet.StageId, out Dictionary<byte, List<EnemySpawn>> stageEnemySpawns);

                if(!stageEnemySpawns.ContainsKey(enemySet.SubGroupId))
                    stageEnemySpawns.Add(enemySet.SubGroupId, new List<EnemySpawn>());

                stageEnemySpawns.TryGetValue(enemySet.SubGroupId, out List<EnemySpawn> stageSubGroupEnemySpawns);

                stageSubGroupEnemySpawns.Add(enemySet);
            }            
        }
    }
}
