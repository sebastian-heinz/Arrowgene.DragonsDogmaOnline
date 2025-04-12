using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IDefaultGatherMixin
    {
        public abstract List<InstancedGatheringItem> GenerateGatheringDrops(GameClient client, StageLayoutId stageLayoutId, uint index);
    }
}
