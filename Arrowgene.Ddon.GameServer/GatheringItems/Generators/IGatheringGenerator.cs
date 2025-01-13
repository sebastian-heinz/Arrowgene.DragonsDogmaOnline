using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    internal interface IGatheringGenerator
    {
        public List<InstancedGatheringItem> Generate(GameClient client, StageId stageId, uint index);
    }
}
