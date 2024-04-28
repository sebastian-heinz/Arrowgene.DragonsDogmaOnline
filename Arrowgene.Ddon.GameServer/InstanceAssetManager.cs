using System.Collections.Generic;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public abstract class InstanceAssetManager<T1, T2, T3>
    {
        public InstanceAssetManager()
        {
            this._instancedAssetsDictionary = new Dictionary<(StageId, T1), List<T3>>();
        }

        private readonly Dictionary<(StageId, T1), List<T3>> _instancedAssetsDictionary;

        public List<T3> GetAssets(CDataStageLayoutId stageLayoutId, T1 subGroupId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<T3> GetAssets(StageId stageId, T1 subGroupId)
        {
            if(!_instancedAssetsDictionary.ContainsKey((stageId, subGroupId)))
            {
                List<T2> items = FetchAssetsFromRepository(stageId, subGroupId);
                List<T3> instancedAssets = InstanceAssets(items);
                _instancedAssetsDictionary.Add((stageId, subGroupId), instancedAssets);
                return instancedAssets;
            }
            return _instancedAssetsDictionary[(stageId, subGroupId)];
        }

        public void Clear()
        {
            _instancedAssetsDictionary.Clear();
        }

        protected abstract List<T2> FetchAssetsFromRepository(StageId stage, T1 subGroupId);

        protected abstract List<T3> InstanceAssets(List<T2> originals);
    }
}