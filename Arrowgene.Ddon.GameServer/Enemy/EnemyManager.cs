using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Enemy
{
    public class EnemyManager : StageLocationAssetManager<EnemySpawn, byte>
    {
        public EnemyManager(AssetRepository assetRepository, IDatabase database)
        : base(assetRepository, AssetRepository.EnemySpawnsKey, database, assetRepository.EnemySpawns)
        {
        }
    }
}
