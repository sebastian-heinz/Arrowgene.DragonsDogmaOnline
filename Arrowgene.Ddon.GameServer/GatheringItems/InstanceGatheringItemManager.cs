using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceGatheringItemManager : InstanceItemManager
    {
        private readonly AssetRepository _assetRepository;

        private Dictionary<(StageId, uint), List<GatheringItem>> BitterBlackLootTables; 

        public InstanceGatheringItemManager(AssetRepository assetRepository)
        {
            this._assetRepository = assetRepository;
            BitterBlackLootTables = new Dictionary<(StageId, uint), List<GatheringItem>>();
        }

        protected override List<GatheringItem> FetchAssetsFromRepository(StageId stage, int subGroupId)
        {
            return _assetRepository.GatheringItems.GetValueOrDefault((stage, (uint)subGroupId)) ?? new List<GatheringItem>();
        }

        protected override IEnumerable<List<GatheringItem>> FetchAssetsFromRepository(StageId stage)
        {
            return _assetRepository.GatheringItems.Where(x => x.Key.Item1.Equals(stage)).Select(x => x.Value);
        }
    }
}
