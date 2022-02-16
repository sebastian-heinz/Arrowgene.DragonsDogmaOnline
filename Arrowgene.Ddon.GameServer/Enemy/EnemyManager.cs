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

        private readonly AssetRepository _assetRepository;
        private readonly IDatabase _database;        

        public EnemyManager(AssetRepository assetRepository, IDatabase database)
        {
            _assetRepository = assetRepository;
            _database = database;
        }

        public List<EnemySpawn> GetSpawns(CStageLayoutID stageLayoutId, byte subGroupId)
        {
            return GetSpawns(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<EnemySpawn> GetSpawns(StageId stageId, byte subGroupId)
        {
            // This is probably a terrible idea but i adore the syntax
            var spawns = 
                from spawn in _assetRepository.EnemySpawns
                where spawn.StageId.Equals(stageId) && spawn.SubGroupId == subGroupId
                select spawn;

            return spawns.ToList<EnemySpawn>();
        }
    }
}
