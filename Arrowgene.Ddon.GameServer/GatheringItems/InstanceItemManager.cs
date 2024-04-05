using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public abstract class InstanceItemManager<T>
    {
        public InstanceItemManager()
        {
            this._gatheringItemsDictionary = new Dictionary<(StageId, T), List<InstancedGatheringItem>>();
        }

        private readonly Dictionary<(StageId, T), List<InstancedGatheringItem>> _gatheringItemsDictionary;

        public List<InstancedGatheringItem> GetAssets(CDataStageLayoutId stageLayoutId, T subGroupId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<InstancedGatheringItem> GetAssets(StageId stageId, T subGroupId)
        {
            if(!_gatheringItemsDictionary.ContainsKey((stageId, subGroupId)))
            {
                List<GatheringItem> items = FetchItemsFromRepository(stageId, subGroupId);
                List<InstancedGatheringItem> instancedItems = items.Select(item => new InstancedGatheringItem(item)).ToList();
                _gatheringItemsDictionary.Add((stageId, subGroupId), instancedItems);
                return instancedItems;
            }
            return _gatheringItemsDictionary[(stageId, subGroupId)];
        }

        public void Clear()
        {
            _gatheringItemsDictionary.Clear();
        }

        protected abstract List<GatheringItem> FetchItemsFromRepository(StageId stage, T subGroupId);
    }
}