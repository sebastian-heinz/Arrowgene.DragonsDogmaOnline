using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Json;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.Shared
{
    public class AssetRepository
    {
        // Client data
        public const string ClientErrorCodesKey = "ClientErrorCodes.csv";
        public const string ItemListKey = "itemlist.csv";

        // Server data
        public const string EnemySpawnsKey = "EnemySpawn.csv";
        public const string GatheringItemsKey = "GatheringItem.csv";
        public const string MyPawnAssetKey = "MyPawn.csv";
        public const string MyRoomAssetKey = "MyRoom.csv";
        public const string ArisenAssetKey = "Arisen.csv";
        public const string StorageKey = "Storage.csv";
        public const string StorageItemKey = "StorageItem.csv";
        public const string ShopKey = "Shop.json";
        public const string ServerListKey = "GameServerList.csv";
        public const string WarpPointsKey = "WarpPoints.csv";
        public const string CraftingRecipesKey = "CraftingRecipes.json";

        private static readonly ILogger Logger = LogProvider.Logger(typeof(AssetRepository));

        public event EventHandler<AssetChangedEventArgs> AssetChanged;

        private readonly DirectoryInfo _directory;
        private readonly Dictionary<string, FileSystemWatcher> _fileSystemWatchers;

        public AssetRepository(string folder)
        {
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                Logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            _fileSystemWatchers = new Dictionary<string, FileSystemWatcher>();

            ClientErrorCodes = new List<CDataErrorMessage>();
            ClientItemInfos = new List<ClientItemInfo>();
            EnemySpawns = new List<EnemySpawn>();
            GatheringItems = new List<GatheringItem>();
            ServerList = new List<CDataGameServerListInfo>();
            MyPawnAsset = new List<MyPawnCsv>();
            MyRoomAsset = new List<MyRoomCsv>();
            ArisenAsset = new List<ArisenCsv>();
            StorageAsset = new List<CDataCharacterItemSlotInfo>();
            StorageItemAsset = new List<Tuple<StorageType, uint, Item>>();
            ShopAsset = new List<Shop>();
            WarpPoints = new List<WarpPoint>();
            CraftingRecipesAsset = new List<S2CCraftRecipeGetCraftRecipeRes>();
        }

        public List<CDataErrorMessage> ClientErrorCodes { get; }
        public List<ClientItemInfo> ClientItemInfos { get; set; } // May be incorrect, or incomplete
        public List<EnemySpawn> EnemySpawns { get; }
        public List<GatheringItem> GatheringItems { get; }
        public List<CDataGameServerListInfo> ServerList { get; }
        public List<MyPawnCsv> MyPawnAsset { get; }
        public List<MyRoomCsv> MyRoomAsset { get; }
        public List<ArisenCsv> ArisenAsset { get; }
        public List<CDataCharacterItemSlotInfo> StorageAsset { get; }
        public List<Tuple<StorageType, uint, Item>> StorageItemAsset { get; }
        public List<Shop> ShopAsset { get; }
        public List<WarpPoint> WarpPoints { get; }
        public List<S2CCraftRecipeGetCraftRecipeRes> CraftingRecipesAsset { get; }

        public void Initialize()
        {
            RegisterAsset(ClientErrorCodes, ClientErrorCodesKey, new ClientErrorCodeCsv());
            RegisterAsset(ClientItemInfos, ItemListKey, new ClientItemInfoCsv());
            RegisterAsset(EnemySpawns, EnemySpawnsKey, new EnemySpawnCsv());
            RegisterAsset(GatheringItems, GatheringItemsKey, new GatheringItemCsv());
            RegisterAsset(MyPawnAsset, MyPawnAssetKey, new MyPawnCsvReader());
            RegisterAsset(MyRoomAsset, MyRoomAssetKey, new MyRoomCsvReader());
            RegisterAsset(ArisenAsset, ArisenAssetKey, new ArisenCsvReader());
            RegisterAsset(ServerList, ServerListKey, new GameServerListInfoCsv());
            RegisterAsset(StorageAsset, StorageKey, new StorageCsv());
            RegisterAsset(StorageItemAsset, StorageItemKey, new StorageItemCsv());
            RegisterAsset(ShopAsset, ShopKey, new JsonReaderWriter<Shop>());
            RegisterAsset(WarpPoints, WarpPointsKey, new WarpPointCsv());
            RegisterAsset(CraftingRecipesAsset, CraftingRecipesKey, new JsonReaderWriter<S2CCraftRecipeGetCraftRecipeRes>());
        }

        private void RegisterAsset<T>(List<T> list, string key, IAssetDeserializer<T> readerWriter)
        {
            Load(list, key, readerWriter);
            RegisterFileSystemWatcher(list, key, readerWriter);
        }

        private void Load<T>(List<T> list, string key, IAssetDeserializer<T> readerWriter)
        {
            string path = Path.Combine(_directory.FullName, key);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                Logger.Error($"Could not load '{key}', file does not exist");
            }

            try {
                List<T> assets = readerWriter.ReadPath(file.FullName);

                list.Clear();
                list.AddRange(assets);
                OnAssetChanged(key, list);
            }
            catch (Exception e)
            {
                Logger.Error($"Could not load '{key}', error reading the file contents");
                Logger.Exception(e);
            }
        }

        private void RegisterFileSystemWatcher<T>(List<T> list, string key, IAssetDeserializer<T> readerWriter)
        {
            if (_fileSystemWatchers.ContainsKey(key))
            {
                return;
            }

            FileSystemWatcher watcher = new FileSystemWatcher(_directory.FullName, key);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += (object sender, FileSystemEventArgs e) =>
            {
                Logger.Debug($"Reloading assets from file '{e.FullPath}'");
                // Try reloading file
                int attempts = 0;
                while (true)
                {
                    try
                    {
                        Load(list, key, readerWriter);
                        break;
                    }
                    catch (IOException ex)
                    {
                        // File isn't ready yet, so we need to keep on waiting until it is.
                        attempts++;
                        Logger.Write(LogLevel.Error, $"Failed to reload {e.FullPath}. {attempts} attempts", ex);

                        if (attempts > 10)
                        {
                            Logger.Write(LogLevel.Error,
                                $"Failed to reload {e.FullPath} after {attempts} attempts. Giving up.", ex);
                            break;
                        }
                    }

                    Thread.Sleep(1000);
                }
            };
            watcher.EnableRaisingEvents = true;
            _fileSystemWatchers.Add(key, watcher);
        }

        private void OnAssetChanged(string key, object asset)
        {
            EventHandler<AssetChangedEventArgs> assetChanged = AssetChanged;
            if (assetChanged != null)
            {
                AssetChangedEventArgs assetChangedEventArgs = new AssetChangedEventArgs(key, asset);
                assetChanged(this, assetChangedEventArgs);
            }
        }
    }
}
