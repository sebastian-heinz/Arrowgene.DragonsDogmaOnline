using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    /// <summary>
    /// Interface for managing various objects that appear in instances:
    ///     Enemies, via InstanceEnemyManager, <Enemy,InstancedEnemy>
    ///     Gatherable items, via InstanceGatheringItemManager, <List<GatheringItem>, List<InstancedGatheringItem>>
    ///     Dropped items, via InstanceDropItemManager, <List<GatheringItem>, List<InstancedGatheringItem>>
    ///     
    /// Notably does not cover enemies associated with quests.
    /// 
    /// Each object is associated uniquely with a stageLayoutId and an index, 
    /// which may also be referred to as setId, posId, or PositionIndex, among other terms.
    /// 
    /// Enemies and dropped items are paired at the same stageLayoutId and index.
    /// An enemy spawned at (foo, bar) will have its drops instanced at (foo, bar) in the other manager.
    /// 
    /// For enemies, the index is NOT the spawn subgroup, but the position index.
    /// </summary>
    public abstract class InstanceAssetManager<TItem, TAssetItem>
    {
        public InstanceAssetManager()
        {
            this._instancedAssetsDictionary = new Dictionary<StageId, Dictionary<int, TAssetItem>>();
        }

        private readonly Dictionary<StageId, Dictionary<int, TAssetItem>> _instancedAssetsDictionary;

        public bool HasAssetsInstanced(CDataStageLayoutId stageLayoutId, int subId)
        {
            var stageId = StageId.FromStageLayoutId(stageLayoutId);
            return HasAssetsInstanced(stageId, subId);
        }

        public bool HasAssetsInstanced(StageId stageId, int subId)
        {
            return _instancedAssetsDictionary.ContainsKey(stageId) && _instancedAssetsDictionary[stageId].ContainsKey(subId);
        }

        public List<TAssetItem> GetAssets(CDataStageLayoutId stageLayoutId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId));
        }

        public List<TAssetItem> GetAssets(StageId stageId)
        {
            if (!_instancedAssetsDictionary.ContainsKey(stageId))
            {
                IEnumerable<TItem> items = FetchAssetsFromRepository(stageId);
                List<TAssetItem> instancedAssets = InstanceAssets(items);
                if (!_instancedAssetsDictionary.ContainsKey(stageId))
                {
                    _instancedAssetsDictionary[stageId] = new Dictionary<int, TAssetItem>();
                }
                for (int i = 0; i < instancedAssets.Count; i++)
                {
                    _instancedAssetsDictionary[stageId].Add(i, instancedAssets[i]);
                }
                return instancedAssets;
            }
            else
            {
                return _instancedAssetsDictionary[stageId].Values.ToList();
            }
        }

        public TAssetItem GetAssets(CDataStageLayoutId stageLayoutId, int subId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), subId);
        }

        public TAssetItem GetAssets(StageId stageId, int subId)
        {
            if(!HasAssetsInstanced(stageId, subId))
            {
                TItem items = FetchAssetsFromRepository(stageId, subId);
                TAssetItem instancedAssets = InstanceAssets(items);

                if (!_instancedAssetsDictionary.ContainsKey(stageId))
                {
                    _instancedAssetsDictionary[stageId] = new Dictionary<int, TAssetItem>();
                }
                _instancedAssetsDictionary[stageId].Add(subId, instancedAssets);
                return instancedAssets;
            }
            return _instancedAssetsDictionary[stageId][subId];
        }

        public virtual void Clear()
        {
            _instancedAssetsDictionary.Clear();
        }

        protected abstract TItem FetchAssetsFromRepository(StageId stage, int subId);

        protected abstract IEnumerable<TItem> FetchAssetsFromRepository(StageId stage);

        protected abstract TAssetItem InstanceAssets(TItem original);

        protected abstract List<TAssetItem> InstanceAssets(IEnumerable<TItem> originals);
    }
}
