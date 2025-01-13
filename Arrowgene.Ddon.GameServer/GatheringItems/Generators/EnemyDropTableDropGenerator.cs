using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    internal class EnemyDropTableDropGenerator : IDropGenerator
    {
        public List<InstancedGatheringItem> Generate(GameClient client, InstancedEnemy enemyKilled)
        {
            return enemyKilled?.DropsTable?.Items.Select(item => new InstancedGatheringItem(item))
                .Where(instancedAsset => instancedAsset.ItemNum > 0)
                .ToList()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_ENEMY_DROP_DATA_NONE);
        }
    }
}
