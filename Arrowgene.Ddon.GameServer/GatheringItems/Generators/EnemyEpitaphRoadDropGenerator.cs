using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    internal class EnemyEpitaphRoadDropGenerator : IDropGenerator
    {
        private readonly DdonGameServer Server;

        // TODO: Make these configurable.
        private readonly List<(EpitaphBuffId BuffId, uint ItemId, uint Amount, double Chance)> gDropConfigs = new List<(EpitaphBuffId BuffId, uint ItemId, uint Amount, double Chance)>()
        {
            (EpitaphBuffId.EnemyGoldDropIncrease,  7789, 100, 0.5),
            (EpitaphBuffId.EnemyRiftDropIncrease,  7792, 100, 0.5),
            (EpitaphBuffId.EnemySoulDropIncrease,     0,   1, 0.5),
        };

        public EnemyEpitaphRoadDropGenerator(DdonGameServer server)
        {
            Server = server;
        }

        public List<InstancedGatheringItem> Generate(GameClient client, InstancedEnemy enemyKilled)
        {
            var stageId = enemyKilled.StageId;
            if (!StageManager.IsEpitaphRoadStageId(stageId))
            {
                return new();
            }

            var dungeonInfo = Server.EpitaphRoadManager.GetDungeonInfoByStageId(stageId);

            List<InstancedGatheringItem> results = new List<InstancedGatheringItem>();

            foreach (var item in Server.EpitaphRoadManager.GetRandomLootForStageId(stageId))
            {
                results.AddRange(Server.EpitaphRoadManager.RollInstancedGatheringItem(item));
            }

            // Drops augmented by dungeon buffs
            var buffs = Server.EpitaphRoadManager.GetPartyBuffs(client.Party);
            foreach (var config in gDropConfigs)
            {
                var buff = GetBuff(buffs, config.BuffId);
                if (buff != null)
                {
                    uint itemId = config.ItemId;
                    if (config.BuffId == EpitaphBuffId.EnemySoulDropIncrease)
                    {
                        itemId = dungeonInfo.SoulItemId;
                    }

                    uint amount = GetAmount(buff, config.Amount, config.Chance);
                    if (amount > 0)
                    {
                        results.Add(new InstancedGatheringItem()
                        {
                            ItemId = itemId,
                            ItemNum = amount
                        });
                    }
                }
            }

            return results;
        }

        private EpitaphBuff GetBuff(List<EpitaphBuff> buffs, EpitaphBuffId buffId)
        {
            return buffs.Where(x => (EpitaphBuffId)x.BuffId == buffId).FirstOrDefault();
        }

        private uint GetAmount(EpitaphBuff buff, uint amount, double chance)
        {
            if (chance >= Random.Shared.NextDouble())
            {
                return (uint)Random.Shared.Next(1, (int)(buff.Increment * (amount + 1)));

            }
            return 0;
        }
    }
}
