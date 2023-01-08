using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer
{
    public class StageLocationAssetManager<T, Y> where T : IStageLocationAsset<Y>
    {
        protected readonly Dictionary<StageId, Dictionary<Y, List<T>>> _assetDictionary;
        protected readonly AssetRepository _assetRepository;
        protected readonly List<T> _assetList;
        protected readonly IDatabase _database;

        public StageLocationAssetManager(AssetRepository assetRepository, IDatabase database, List<T> assetList)
        {
            _assetRepository = assetRepository;
            _database = database;
            _assetList = assetList;
            _assetDictionary = new Dictionary<StageId, Dictionary<Y, List<T>>>();

            Load();
            _assetRepository.AssetChanged += AssetRepositoryOnAssetChanged;
        }

        public List<T> GetAssets(CDataStageLayoutId stageLayoutId, Y subGroupId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<T> GetAssets(StageId stageId, Y subGroupId)
        {
            if (!_assetDictionary.ContainsKey(stageId))
            {
                return new List<T>();
            }

            Dictionary<Y, List<T>> stageAssets = _assetDictionary[stageId];
            if (!stageAssets.ContainsKey(subGroupId))
            {
                return new List<T>();
            }

            // TODO check time of day

            return new List<T>(stageAssets[subGroupId]);
        }

        public void Load()
        {
            _assetDictionary.Clear();
            foreach (T asset in _assetList)
            {
                Dictionary<Y, List<T>> stageAssets;
                if (_assetDictionary.ContainsKey(asset.StageId))
                {
                    stageAssets = _assetDictionary[asset.StageId];
                }
                else
                {
                    stageAssets = new Dictionary<Y, List<T>>();
                    _assetDictionary.Add(asset.StageId, stageAssets);
                }

                List<T> subGroupAssets;
                if (stageAssets.ContainsKey(asset.SubGroupId))
                {
                    subGroupAssets = stageAssets[asset.SubGroupId];
                }
                else
                {
                    subGroupAssets = new List<T>();
                    stageAssets.Add(asset.SubGroupId, subGroupAssets);
                }

                subGroupAssets.Add(asset);
            }
        }
        
        private void AssetRepositoryOnAssetChanged(object sender, AssetChangedEventArgs e)
        {
            if (e.Key == AssetRepository.EnemySpawnsKey)
            {
                Load();
            }
        }
    }
}