using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class GatheringItemManager : StageLocationAssetManager<GatheringItem, uint>
    {
        public GatheringItemManager(AssetRepository assetRepository, IDatabase database) : base(assetRepository, database, assetRepository.GatheringItems)
        {
        }
    }
}