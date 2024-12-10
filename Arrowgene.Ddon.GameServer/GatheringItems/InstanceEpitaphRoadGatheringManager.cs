using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceEpitaphRoadGatheringManager
    {
        private Dictionary<(StageId, uint), List<InstancedGatheringItem>> GatheringLootTables;

        private DdonGameServer Server;

        public InstanceEpitaphRoadGatheringManager(DdonGameServer server)
        {
            Server = server;
            GatheringLootTables = new Dictionary<(StageId, uint), List<InstancedGatheringItem>>();
        }

        public List<InstancedGatheringItem> FetchItems(GameClient client, StageId stageId, uint posId)
        {
            if (!HasLootGenerated(stageId, posId))
            {
                var items = Server.EpitaphRoadManager.RollGatheringLoot(client, client.Character, stageId, posId);
                AddLootTable(stageId, posId, items);
            }

            return GatheringLootTables[(stageId, posId)];
        }

        private bool HasLootGenerated(StageId stageId, uint posId)
        {
            return GatheringLootTables.ContainsKey((stageId, posId));
        }

        public List<InstancedGatheringItem> AddLootTable(StageId stageId, uint posId, List<InstancedGatheringItem> loot)
        {
            GatheringLootTables[(stageId, posId)] = loot;
            return loot;
        }

        public void Reset()
        {
            GatheringLootTables.Clear();
        }
    }
}
