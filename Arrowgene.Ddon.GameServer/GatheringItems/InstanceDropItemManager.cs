using System.Collections.Generic;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceDropItemManager : InstanceItemManager<uint>
    {
        public InstanceDropItemManager(AssetRepository assetRepository) : base()
        {
            this.assetRepository = assetRepository;
        }

        private readonly AssetRepository assetRepository;

        protected override List<GatheringItem> FetchItemsFromRepository(StageId stage, uint setId)
        {
            List<Enemy> enemiesInSet = assetRepository.EnemySpawnAsset.Enemies.GetValueOrDefault((stage, (byte) 0));
            if(enemiesInSet != null && setId < enemiesInSet.Count)
            {
                Enemy enemy = enemiesInSet[(int) setId];
                if(enemy.DropsTable != null)
                {
                    return enemy.DropsTable.Items;
                }
            }
            return new List<GatheringItem>();
        }
    }
}