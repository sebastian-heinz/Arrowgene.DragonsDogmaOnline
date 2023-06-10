using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.GameServer
{
    public abstract class AssetManager<T>
    {
        protected readonly AssetRepository _assetRepository;
        protected readonly string _assetKey;
        protected readonly IDatabase _database;
        protected readonly List<T> _assetList;

        protected AssetManager(AssetRepository assetRepository, string assetKey, IDatabase database, List<T> assetList)
        {
            _assetRepository = assetRepository;
            _assetKey = assetKey;
            _database = database;
            _assetList = assetList;

            OnInit();
            Load();
            _assetRepository.AssetChanged += AssetRepositoryOnAssetChanged;
        }

        protected abstract void OnInit();

        public abstract void Load();

        private void AssetRepositoryOnAssetChanged(object sender, AssetChangedEventArgs e)
        {
            if(e.Key == this._assetKey)
            {
                Load();
            }
        }
    }
}