using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    public class BitterblackGatheringItemGenerator : IGatheringGenerator
    {
        public BitterblackGatheringItemGenerator(DdonGameServer server)
        {
            Server = server;
        }

        DdonGameServer Server { get; set; }

        public override List<InstancedGatheringItem> Generate(GameClient client, StageLayoutId stageId, uint index)
        {
            if (!StageManager.IsBitterBlackMazeStageId(stageId))
            {
                return new();
            }

            return BitterblackMazeManager.RollChestLoot(Server, client.Character, stageId, index);
        }
    }
}
