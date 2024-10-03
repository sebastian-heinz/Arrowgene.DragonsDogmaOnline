using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceDropItemManager : InstanceItemManager
    {
        private readonly GameClient _client;

        public InstanceDropItemManager(GameClient client)
        {
            this._client = client;
        }

        protected override List<GatheringItem> FetchAssetsFromRepository(StageId stage, int setId)
        {
            List<InstancedEnemy> enemiesInSet =  _client.Party.InstanceEnemyManager.GetAssets(stage);
            if(enemiesInSet != null && setId < enemiesInSet.Count)
            {
                Enemy enemy = enemiesInSet[(int) setId];

                if (enemy.DropsTable != null)
                {
                    return enemy.DropsTable.Items;
                }
            }
            return new List<GatheringItem>();
        }

        protected override IEnumerable<List<GatheringItem>> FetchAssetsFromRepository(StageId stage)
        {
            List<InstancedEnemy> enemiesInSet = _client.Party.InstanceEnemyManager.GetAssets(stage);
            return enemiesInSet.Select(x => x.DropsTable.Items);
        }
    }
}
