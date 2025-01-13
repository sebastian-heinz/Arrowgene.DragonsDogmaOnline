using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    internal interface IDropGenerator
    {
        public List<InstancedGatheringItem> Generate(GameClient client, InstancedEnemy enemyKilled);
    }
}
