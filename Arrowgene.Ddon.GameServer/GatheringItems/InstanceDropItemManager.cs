using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

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

            //Logger.Debug($"enemiesInSet is not null? {enemiesInSet != null}");
            Console.WriteLine("======================================= InstanceDropItemManager ==========================================");
            Console.WriteLine($"enemiesInSet is not null? {enemiesInSet != null}");
            Console.WriteLine($"setId ({setId}) < enemiesInSet.Count ({enemiesInSet.Count}) ? {setId < enemiesInSet.Count}");

            if(enemiesInSet != null && setId < enemiesInSet.Count)
            {
                Enemy enemy = enemiesInSet[(int) setId];
                Console.WriteLine($"enemy IS {enemy.EnemyId}");
                Console.WriteLine($"enemy.DropsTable is null? {enemy.DropsTable}");

                if (enemy.DropsTable != null)
                {
                    Console.WriteLine($"enemy.DropsTable.Items:");

                    foreach (var item in enemy.DropsTable.Items)
                    {
                        Console.WriteLine($"Item: {item}");
                    }

                    return enemy.DropsTable.Items;
                }
            }
            return new List<GatheringItem>();
        }
    }
}
