using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer
{
    public class StageLocationAssetManager<T, Y> : AssetManager<T> where T : IStageLocationAsset<Y>
    {
        protected MultiKeyMultiValueDictionary<StageId, Y, T> _assetDictionary;

        public StageLocationAssetManager(AssetRepository assetRepository, string assetKey, IDatabase database, List<T> assetList) : base(assetRepository, assetKey, database, assetList)
        {
        }

        protected override void OnInit()
        {
            _assetDictionary = new MultiKeyMultiValueDictionary<StageId, Y, T>();
        }

        public override void Load()
        {
            _assetDictionary.Clear();
            foreach (T asset in _assetList)
            {
                _assetDictionary.Add(asset.StageId, asset.SubGroupId, asset);
            }
        }

        public List<T> GetAssets(CDataStageLayoutId stageLayoutId, Y subGroupId)
        {
            return GetAssets(StageId.FromStageLayoutId(stageLayoutId), subGroupId);
        }

        public List<T> GetAssets(StageId stageId, Y subGroupId)
        {
            return _assetDictionary.Get(stageId, subGroupId);
        }
    }
}