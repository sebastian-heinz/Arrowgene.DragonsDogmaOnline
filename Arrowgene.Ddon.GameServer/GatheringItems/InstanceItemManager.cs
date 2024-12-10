using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.GatheringItems;
using Arrowgene.Ddon.Shared.Model;

public abstract class InstanceItemManager : InstanceAssetManager<List<GatheringItem>, List<InstancedGatheringItem>>
{
    protected override List<InstancedGatheringItem> InstanceAssets(List<GatheringItem> originals)
    {
        return originals.Select(item => new InstancedGatheringItem(item))
            .Where(instancedAsset => instancedAsset.ItemNum > 0)
            .ToList();
    }

    protected override List<List<InstancedGatheringItem>> InstanceAssets(IEnumerable<List<GatheringItem>> originals)
    {
        return originals.Select(x => InstanceAssets(x)).ToList();
    }
}
