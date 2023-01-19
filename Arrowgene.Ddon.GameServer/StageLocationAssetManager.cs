using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer
{
    public class StageLocationAssetManager<T, Y> where T : IStageLocationAsset<Y>
    {
        protected readonly MultiKeyMultiValueDictionary<StageId, Y, T> _assetDictionary;
        protected readonly AssetRepository _assetRepository;
        protected readonly string _assetKey;
        protected readonly List<T> _assetList;
        protected readonly IDatabase _database;

        public StageLocationAssetManager(AssetRepository assetRepository, string assetKey, IDatabase database, List<T> assetList)
        {
            _assetRepository = assetRepository;
            _assetKey = assetKey;
            _database = database;
            _assetList = assetList;
            _assetDictionary = new MultiKeyMultiValueDictionary<StageId, Y, T>();

            Load();
            _assetRepository.AssetChanged += AssetRepositoryOnAssetChanged;
        }

        public List<T> GetAssets(CDataStageLayoutId stageLayoutId, Y subGroupId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<T> GetAssets(StageId stageId, Y subGroupId)
        {
            return _assetDictionary.Get(stageId, subGroupId);
        }

        public void Load()
        {
            _assetDictionary.Clear();
            foreach (T asset in _assetList)
            {
                _assetDictionary.Add(asset.StageId, asset.SubGroupId, asset);
            }
        }
        
        private void AssetRepositoryOnAssetChanged(object sender, AssetChangedEventArgs e)
        {
            if(e.Key == this._assetKey)
            {
                Load();
            }
        }
    }
}