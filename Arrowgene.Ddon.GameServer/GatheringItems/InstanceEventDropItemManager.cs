using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceEventDropItemManager
    {
        private Dictionary<(StageId, uint), List<InstancedGatheringItem>> EventLootTables;
        private AssetRepository AssetRepository;

        public InstanceEventDropItemManager(AssetRepository assetRepository)
        {
            EventLootTables = new Dictionary<(StageId, uint), List<InstancedGatheringItem>>();
            AssetRepository = assetRepository;
        }

        private bool DropEnabled(Character character, EventItem item, Enemy enemy, StageId stageId)
        {
            if (item.QuestIds.Count > 0 && !item.QuestIds.Any(x => QuestManager.IsQuestEnabled(x)))
            {
                return false;
            }

            if (item.StageIds.Count > 0 && !item.StageIds.Contains(stageId.Id))
            {
                return false;
            }

            if (item.EnemyIds.Count > 0 && !item.EnemyIds.Contains(enemy.Id))
            {
                return false;
            }

            if (item.RequiredItemsEquipped.Count > 0)
            {
                if (item.ItemConstraint == EventItemConstraint.All)
                {
                    foreach (var itemId in item.RequiredItemsEquipped)
                    {
                        if (!character.Equipment.GetItems(EquipType.Performance).Exists(x => x?.ItemId == itemId) &&
                            !character.Equipment.GetItems(EquipType.Visual).Exists(x => x?.ItemId == itemId))
                        {
                            return false;
                        }
                    }
                }
                else if (item.ItemConstraint == EventItemConstraint.AtLeastOne)
                {
                    bool foundItem = false;
                    foreach (var itemId in item.RequiredItemsEquipped)
                    {
                        if (character.Equipment.GetItems(EquipType.Performance).Exists(x => x?.ItemId == itemId) ||
                            character.Equipment.GetItems(EquipType.Visual).Exists(x => x?.ItemId == itemId))
                        {
                            foundItem = true;
                            break;
                        }
                    }

                    if (!foundItem)
                    {
                        return false;
                    }
                }
            }

            if (item.RequiresLanternLit && !character.IsLanternLit)
            {
                return false;
            }

            return true;
        }

        public List<InstancedGatheringItem> FetchEventItems(GameClient client, CDataStageLayoutId layoutId, uint posId)
        {
            var stageId = layoutId.AsStageId();
            if (!EventLootTables.ContainsKey((stageId, posId)))
            {
                return new List<InstancedGatheringItem>();
            }

            return EventLootTables[(stageId, posId)];
        }

        public List<InstancedGatheringItem> GenerateEventItems(GameClient client, Enemy enemy, CDataStageLayoutId layoutId, uint posId)
        {
            var stageId = layoutId.AsStageId();
            if (!HasEventLootGenerated(stageId, posId))
            {
                List<InstancedGatheringItem> results = new List<InstancedGatheringItem>();
                foreach (var item in AssetRepository.EventDropsAsset.EventItems)
                {
                    if (!DropEnabled(client.Character, item, enemy, stageId))
                    {
                        continue;
                    }

                    if (item.Chance > Random.Shared.NextDouble())
                    {
                        results.Add(new InstancedGatheringItem()
                        {
                            ItemId = item.ItemId,
                            ItemNum = (uint) Random.Shared.Next((int) item.MinAmount, (int)(item.MaxAmount + 1)),
                        });
                    }
                }
                EventLootTables[(stageId, posId)] = results;
            }

            return EventLootTables[(stageId, posId)];
        }

        private bool HasEventLootGenerated(StageId stageId, uint posId)
        {
            return EventLootTables.ContainsKey((stageId, posId));
        }

        public List<InstancedGatheringItem> AddEventItemTable(StageId stageId, uint posId, List<InstancedGatheringItem> loot)
        {
            EventLootTables[(stageId, posId)] = loot;
            return loot;
        }

        public void Reset()
        {
            EventLootTables.Clear();
        }
    }
}
