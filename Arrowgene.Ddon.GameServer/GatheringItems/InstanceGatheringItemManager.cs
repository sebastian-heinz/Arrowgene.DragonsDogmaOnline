using Arrowgene.Ddon.GameServer.GatheringItems.Generators;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceGatheringItemManager
    {
        private readonly Dictionary<StageId, Dictionary<uint, List<InstancedGatheringItem>>> InstancedItems;

        private readonly GameClient Client;
        private readonly DdonGameServer Server;
        private readonly List<IGatheringGenerator> Generators;

        public InstanceGatheringItemManager(GameClient client, DdonGameServer server)
        {
            Client = client;
            Server = server;
            InstancedItems = new();
            Generators = new()
            {
                new GatheringTableGatheringItemGenerator(server),
                new BitterblackGatheringItemGenerator(server),
                new EpitaphRoadGatheringItemGenerator(server)
            };
        }

        public Dictionary<Type, List<InstancedGatheringItem>> Generate(StageId stageId, uint index)
        {
            return Generators.ToDictionary(key => key.GetType(), val => val.Generate(Client, stageId, index));
        }

        private uint Assign(StageId stageId, uint index, List<InstancedGatheringItem> items)
        {
            uint currentIndex = index;
            if (InstancedItems.TryGetValue(stageId, out var stageItems))
            {
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

        public (bool New, List<InstancedGatheringItem> Items) FetchOrGenerate(StageId stageId, uint index)
        {
            if (InstancedItems.TryGetValue(stageId, out var stageItems) 
                && stageItems.TryGetValue(index, out var returnItems))
            {
                return (false, returnItems);
            }
            else
            {
                var items = Generate(stageId, index).SelectMany(x => x.Value).ToList();
                Assign(stageId, index, items);
                return (true, items);
            }
        }

        public (bool New, List<InstancedGatheringItem> Items) FetchOrGenerate(CDataStageLayoutId stageLayout, uint index)
        {
            return FetchOrGenerate(StageId.FromStageLayoutId(stageLayout), index);
        }

        public void Clear()
        {
            InstancedItems.Clear();
        }

        public string Report(StageId stageId, uint index)
        {
            var infoStrings = FetchOrGenerate(stageId, index).Items.Select(x => $"{ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, x.ItemId).Name} x{x.ItemNum}");
            return string.Join("\n\t", infoStrings);
        }

        public string Report(Dictionary<Type, List<InstancedGatheringItem>> generateResult)
        {
            var infoStrings = generateResult.SelectMany(t => t.Value.Select(x => $"{ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, x.ItemId).Name}\tx{x.ItemNum}\t({t.Key.Name})"));
            return string.Join("\n\t", infoStrings);
        }
    }
}
