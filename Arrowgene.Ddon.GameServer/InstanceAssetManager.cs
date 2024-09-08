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

        public bool HasAssetsInstanced(CDataStageLayoutId stageLayoutId, T1 setId)
        {
            return _instancedAssetsDictionary.ContainsKey((StageId.FromStageLayoutId(stageLayoutId), setId));
        }

        public bool HasAssetsInstanced(StageId stageId, T1 setId)
        {
            return _instancedAssetsDictionary.ContainsKey((stageId, setId));
        }

        public List<T3> GetAssets(CDataStageLayoutId stageLayoutId, T1 setId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), setId);
        }

        public List<T3> GetAssets(StageId stageId, T1 setId)
        {
            if(!HasAssetsInstanced(stageId, setId))
            {
                List<T2> items = FetchAssetsFromRepository(stageId, setId);
                List<T3> instancedAssets = InstanceAssets(items);
                _instancedAssetsDictionary.Add((stageId, setId), instancedAssets);
                return instancedAssets;
            }
            return _instancedAssetsDictionary[(stageId, setId)];
        }

        public virtual void Clear()
        {
            _instancedAssetsDictionary.Clear();
        }

        protected abstract List<T2> FetchAssetsFromRepository(StageId stage, T1 setId);

        protected abstract List<T3> InstanceAssets(List<T2> originals);
    }
}
