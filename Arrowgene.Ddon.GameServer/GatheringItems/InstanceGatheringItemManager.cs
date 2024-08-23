using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

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

        protected override List<GatheringItem> FetchAssetsFromRepository(StageId stage, uint subGroupId)
        {
            return _assetRepository.GatheringItems.GetValueOrDefault((stage, subGroupId)) ?? new List<GatheringItem>();
        }
    }
}
