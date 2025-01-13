using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    internal class EnemyEventDropGenerator : IDropGenerator
    {
        private readonly DdonGameServer Server;

        public EnemyEventDropGenerator(DdonGameServer server)
        {
            Server = server;
        }

        private bool DropEnabled(GameClient client, EventItem item, InstancedEnemy enemy)
        {
            var stageId = enemy.StageId;
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

        private bool EvaluateEmLvConstraint(EventItem item, InstancedEnemy enemy)
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

        private bool EvaluateEmClassConstraint(EventItem item, InstancedEnemy enemy)
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

        public List<InstancedGatheringItem> Generate(GameClient client, InstancedEnemy enemyKilled)
        {
            List<InstancedGatheringItem> results = new List<InstancedGatheringItem>();
            foreach (var item in Server.AssetRepository.EventDropsAsset.EventItems)
            {
                if (!DropEnabled(client, item, enemyKilled))
                {
                    continue;
                }

                if (item.Chance > Random.Shared.NextDouble())
                {
                    results.Add(new InstancedGatheringItem()
                    {
                        ItemId = item.ItemId,
                        ItemNum = (uint)Random.Shared.Next((int)item.MinAmount, (int)(item.MaxAmount + 1)),
                    });
                }
            }
            return results;
        }
    }
}
