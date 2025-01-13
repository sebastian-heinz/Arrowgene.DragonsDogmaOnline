using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceBitterblackGatheringItemManager
    {
        private Dictionary<(StageId, uint), List<InstancedGatheringItem>> BitterBlackLootTables;

        public InstanceBitterblackGatheringItemManager()
        {
            BitterBlackLootTables = new Dictionary<(StageId, uint), List<InstancedGatheringItem>>();
        }

        public List<InstancedGatheringItem> FetchBitterblackItems(DdonGameServer server, GameClient client, StageId stageId, uint posId)
        {
            if (!HasBitterblackLootGenerated(stageId, posId))
            {
                var items = BitterblackMazeManager.RollChestLoot(server, client.Character, stageId, posId);
                client.InstanceBbmGatheringItemManager.AddBitterblackLootTable(stageId, posId, items);
            }

            return BitterBlackLootTables[(stageId, posId)];
        }

        private bool HasBitterblackLootGenerated(StageId stageId, uint posId)
        {
            return BitterBlackLootTables.ContainsKey((stageId, posId));
        }

        public List<InstancedGatheringItem> AddBitterblackLootTable(StageId stageId, uint posId, List<InstancedGatheringItem> loot)
        {
            BitterBlackLootTables[(stageId, posId)] = loot;
            return loot;
        }

        public void Reset()
        {
            BitterBlackLootTables.Clear();
        }
    }
}
