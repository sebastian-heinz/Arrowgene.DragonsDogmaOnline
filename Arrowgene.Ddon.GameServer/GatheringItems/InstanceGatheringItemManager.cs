using System.Collections.Generic;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceGatheringItemManager : InstanceItemManager<uint>
    {
        public InstanceGatheringItemManager(AssetRepository assetRepository) : base()
        {
            this.assetRepository = assetRepository;
        }

        private readonly AssetRepository assetRepository;

        protected override List<GatheringItem> FetchItemsFromRepository(StageId stage, uint subGroupId)
        {
            return assetRepository.GatheringItems.GetValueOrDefault((stage, subGroupId)) ?? new List<GatheringItem>();
        }
    }
}