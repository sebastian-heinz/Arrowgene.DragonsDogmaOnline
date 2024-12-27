using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceEventDropItemManager
    {
        private DdonGameServer Server;
        private Dictionary<(StageId, uint), List<InstancedGatheringItem>> EventLootTables;
        private AssetRepository AssetRepository;

        public InstanceEventDropItemManager(DdonGameServer server)
        {
            Server = server;
            EventLootTables = new Dictionary<(StageId, uint), List<InstancedGatheringItem>>();
            AssetRepository = Server.AssetRepository;
        }

        private bool DropEnabled(GameClient client, EventItem item, Enemy enemy, StageId stageId)
        {
            if (item.QuestIds.Count > 0 && !item.QuestIds.Any(x => QuestManager.GetQuestByScheduleId(x).IsActive(Server, client)))
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

            if (item.EmLvConstraint != EventItemConstraint.None && !EvaluateEmLvConstraint(item, enemy))
            {
                return false;
            }

            if (!EvaluateEmClassConstraint(item, enemy))
            {
                return false;
            }

            if (item.RequiredItemsEquipped.Count > 0)
            {
                if (item.ItemConstraint == EventItemConstraint.All)
                {
                    foreach (var itemId in item.RequiredItemsEquipped)
                    {
                        if (!client.Character.Equipment.GetItems(EquipType.Performance).Exists(x => x?.ItemId == itemId) &&
                            !client.Character.Equipment.GetItems(EquipType.Visual).Exists(x => x?.ItemId == itemId))
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
                        if (client.Character.Equipment.GetItems(EquipType.Performance).Exists(x => x?.ItemId == itemId) ||
                            client.Character.Equipment.GetItems(EquipType.Visual).Exists(x => x?.ItemId == itemId))
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

            if (item.RequiresLanternLit && !client.Character.IsLanternLit)
            {
                return false;
            }

            return true;
        }

        private bool EvaluateEmLvConstraint(EventItem item, Enemy enemy)
        {
            if (item.EmLvConstraint == EventItemConstraint.InRange)
            {
                return enemy.Lv >= item.EmLvConstraintParams.MinLv && enemy.Lv <= item.EmLvConstraintParams.MaxLv;
            }
            else if (item.EmLvConstraint == EventItemConstraint.LessThan)
            {
                return enemy.Lv < item.EmLvConstraintParams.Lv;
            }
            else if (item.EmLvConstraint == EventItemConstraint.LessThanOrEqual)
            {
                return enemy.Lv <= item.EmLvConstraintParams.Lv;
            }
            else if (item.EmLvConstraint == EventItemConstraint.GreaterThan)
            {
                return enemy.Lv > item.EmLvConstraintParams.Lv;
            }
            else if (item.EmLvConstraint == EventItemConstraint.GreaterThanOrEqual)
            {
                return enemy.Lv >= item.EmLvConstraintParams.Lv;
            }

            // An invalid constraint type was passed
            return false;
        }

        private bool EvaluateEmClassConstraint(EventItem item, Enemy enemy)
        {
            if (item.EmClassConstraint == EventItemConstraint.None)
            {
                return true;
            }
            else if (item.EmClassConstraint == EventItemConstraint.IsBoss)
            {
                return enemy.IsBossGauge;
            }
            else if (item.EmClassConstraint == EventItemConstraint.IsNotBoss)
            {
                return !enemy.IsBossGauge;
            }

            // An invalid constraint type was passed 
            return false;
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
                    if (!DropEnabled(client, item, enemy, stageId))
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
