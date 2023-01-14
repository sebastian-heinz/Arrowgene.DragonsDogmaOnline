using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceGatheringItemManager
    {
        public InstanceGatheringItemManager(GatheringItemManager gatheringItemManager)
        {
            this._gatheringItemManager = gatheringItemManager;
            this._gatheringItemsDictionary = new MultiKeyMultiValueDictionary<StageId, uint, GatheringItem>();
        }

        private readonly GatheringItemManager _gatheringItemManager;
        private readonly MultiKeyMultiValueDictionary<StageId, uint, GatheringItem> _gatheringItemsDictionary;

        public List<GatheringItem> GetAssets(CDataStageLayoutId stageLayoutId, uint subGroupId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<GatheringItem> GetAssets(StageId stageId, uint subGroupId)
        {
            if(!_gatheringItemsDictionary.Has(stageId, subGroupId))
            {
                List<GatheringItem> items = _gatheringItemManager.GetAssets(stageId, subGroupId);
                List<GatheringItem> itemsClone = items.Select(item => (GatheringItem) item.Clone()).ToList();
                _gatheringItemsDictionary.AddRange(stageId, subGroupId, itemsClone);
                return items;
            }
            else
            {
                return _gatheringItemsDictionary.Get(stageId, subGroupId);
            }
        }

        public void Clear()
        {
            _gatheringItemsDictionary.Clear();
        }
    }
}