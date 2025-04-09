using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    public abstract class IGatheringGenerator
    {
        public virtual bool IsEnabled()
        {
            return true;
        }

        public abstract List<InstancedGatheringItem> Generate(GameClient client, StageLayoutId stageId, uint index);
    }
}
