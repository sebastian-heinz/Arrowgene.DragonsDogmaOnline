using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    public class GatheringTableGatheringItemGenerator : IGatheringGenerator
    {
        private readonly DdonGameServer Server;

        public GatheringTableGatheringItemGenerator(DdonGameServer server)
        {
            Server = server;
        }

        public List<InstancedGatheringItem> Generate(GameClient client, StageId stageId, uint index)
        {
            if (StageManager.IsBitterBlackMazeStageId(stageId) || StageManager.IsEpitaphRoadStageId(stageId))
            {
                return new();
            }

            return (Server.AssetRepository.GatheringItems.GetValueOrDefault((stageId, index)) ?? new List<GatheringItem>())
                .Select(item => new InstancedGatheringItem(item))
                .Where(instancedAsset => instancedAsset.ItemNum > 0)
                .ToList();
        }
    }
}
