using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceDropItemManager : InstanceItemManager
    {
        private readonly GameClient _client;

        public InstanceDropItemManager(GameClient client)
        {
            this._client = client;
        }

        protected override List<GatheringItem> FetchAssetsFromRepository(StageId stage, uint setId)
        {
            List<InstancedEnemy> enemiesInSet =  _client.Party.InstanceEnemyManager.GetAssets(stage, 0);
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
    }
}
