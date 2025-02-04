using Arrowgene.Ddon.GameServer.GatheringItems.Generators;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceDropItemManager
    {
        private readonly Dictionary<StageId, Dictionary<uint, List<InstancedGatheringItem>>> InstancedItems;

        private static readonly uint COLLISION_OFFSET = 1000;
        private readonly GameClient Client;
        private readonly DdonGameServer Server;
        private readonly List<IDropGenerator> Generators;

        public InstanceDropItemManager(GameClient client, DdonGameServer server)
        {
            Client = client;
            Server = server;
            InstancedItems = new();
            Generators = new()
            {
                new EnemyDropTableDropGenerator(),
                new EnemyEpitaphRoadDropGenerator(server),
                new EnemyEventDropGenerator(server)
            };
        }

        public Dictionary<Type, List<InstancedGatheringItem>> Generate(InstancedEnemy enemy)
        {
            return Generators.ToDictionary(key => key.GetType(), val => val.Generate(Client, enemy));
        }

        public uint Assign(StageId stageId, uint index, List<InstancedGatheringItem> items, bool force = false)
        {
            uint currentIndex = index;
            if (InstancedItems.TryGetValue(stageId, out var stageItems))
            {
                while (stageItems.ContainsKey(currentIndex) && !force)
                {
                    currentIndex += COLLISION_OFFSET;
                }
                InstancedItems[stageId][currentIndex] = items;
            }
            else
            {
                InstancedItems[stageId] = new()
                {
                    { currentIndex, items }
                };
            }
            return currentIndex;
        }

        public uint Assign(CDataStageLayoutId stageLayout, uint index, List<InstancedGatheringItem> items, bool force = false)
        {
            return Assign(StageId.FromStageLayoutId(stageLayout), index, items, force);
        }

        public List<InstancedGatheringItem> Fetch(StageId stageId, uint index)
        {
            if (InstancedItems.TryGetValue(stageId, out var stageItems) 
                && stageItems.TryGetValue(index, out var returnItems))
            {
                return returnItems;
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_DROP_MISSING, $"Missing drop information for {stageId}.{index}");
            }
        }

        public List<InstancedGatheringItem> Fetch(CDataStageLayoutId stageLayout, uint index)
        {
            return Fetch(StageId.FromStageLayoutId(stageLayout), index);
        }

        public void Clear()
        {
            InstancedItems.Clear();
        }

        public string Report(StageId stageId, uint index)
        {
            var infoStrings = Fetch(stageId, index).Select(x => $"{ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, x.ItemId).Name} x{x.ItemNum}");
            return string.Join("\n\t", infoStrings);
        }

        public string Report(Dictionary<Type, List<InstancedGatheringItem>> generateResult)
        {
            var infoStrings = generateResult.SelectMany(t => t.Value.Select(x => $"{ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, x.ItemId).Name}\tx{x.ItemNum}\t({t.Key.Name})"));
            return string.Join("\n\t", infoStrings);
        }
    }
}
