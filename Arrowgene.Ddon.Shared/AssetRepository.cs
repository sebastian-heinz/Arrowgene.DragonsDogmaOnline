using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.AssetReader;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Arrowgene.Ddon.Shared
{
    public class AssetRepository
    {
        public string AssetsPath { get; private set; }

        // Client data
        public const string ClientErrorCodesKey = "ClientErrorCodes.json";
        public const string ItemListKey = "itemlist.csv";

        // Server data
        public const string NamedParamsKey = "named_param.ndp.json";
        public const string EnemySpawnsKey = "EnemySpawn.json";
        public const string GatheringItemsKey = "GatheringItem.csv";
        public const string MyPawnAssetKey = "MyPawn.csv";
        public const string MyRoomAssetKey = "MyRoom.csv";
        public const string ArisenAssetKey = "Arisen.csv";
        public const string PawnStartGearKey = "PawnStartGear.csv";
        public const string StorageKey = "Storage.csv";
        public const string StorageItemKey = "StorageItem.csv";
        public const string ShopKey = "Shop.json";
        public const string ServerListKey = "GameServerList.csv";
        public const string WarpPointsKey = "WarpPoints.csv";
        public const string CraftingRecipesKey = "CraftingRecipes.json";
        public const string CraftingRecipesGradeUpKey = "CraftingRecipesGradeUp.json";
        public const string LearnedNormalSkillsKey = "LearnedNormalSkills.json";
        public const string GPCourseInfoKey = "GpCourseInfo.json";
        public const string SecretAbilityKey = "DefaultSecretAbilities.json";
        public const string CostExpScalingInfoKey = "CostExpScalingInfo.json";
        public const string JobValueShopKey = "JobValueShop.csv";
        public const string StampBonusKey = "StampBonus.csv";
        public const string SpecialShopKey = "SpecialShops.json";
        public const string PawnCostReductionKey = "PawnCostReduction.json";
        public const string PawnCraftSkillCostRateKey = "PawnCraftSkillCostRate.csv";
        public const string PawnCraftSkillSpeedRateKey = "PawnCraftSkillSpeedRate.csv";
        public const string PawnCraftMasterLegendKey = "CraftingLegendPawns.json";
        public const string BitterblackMazeKey = "BitterblackMaze.json";
        public const string QuestDropItemsKey = "QuestEnemyDrops.json";
        public const string RecruitmentBoardCategoryKey = "RecruitmentGroups.json";
        public const string EventDropsKey = "EventDrops.json";
        public const string BonusDungeonKey = "BonusDungeon.json";
        public const string ClanShopKey = "ClanShop.csv";
        public const string EpitaphRoadKey = "EpitaphRoad.json";
        public const string LoadingInfoKey = "LoadingInfo.json";
        public const string AreaRankSpotInfoKey = "AreaRankSpotInfo.csv";
        public const string AreaRankSupplyKey = "AreaRankSupply.json";
        public const string AreaRankRequirementKey = "AreaRankRequirements.json";

        public const string QuestAssestKey = "quests";
        public const string EpitaphAssestKey = "epitaph";

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

            AssetsPath = folder;

            _fileSystemWatchers = new Dictionary<string, FileSystemWatcher>();

            ClientErrorCodes = new Dictionary<ErrorCode, ClientErrorCode>();
            ClientItemInfos = new Dictionary<uint, ClientItemInfo>();
            NamedParamAsset = new Dictionary<uint, NamedParam>();
            EnemySpawnAsset = new EnemySpawnAsset();
            GatheringItems = new Dictionary<(StageLayoutId, uint), List<GatheringItem>>();
            ServerList = new List<ServerInfo>();
            MyPawnAsset = new List<MyPawnCsv>();
            MyRoomAsset = new List<MyRoomCsv>();
            ArisenAsset = new List<ArisenCsv>();
            PawnStartGearAsset = new List<PawnStartGearCsv>();
            StorageAsset = new List<CDataCharacterItemSlotInfo>();
            StorageItemAsset = new List<Tuple<StorageType, uint, Item>>();
            ShopAsset = new List<Shop>();
            WarpPoints = new List<WarpPoint>();
            CraftingRecipesAsset = new List<S2CCraftRecipeGetCraftRecipeRes>();
            CraftingGradeUpRecipesAsset = new List<S2CCraftRecipeGetCraftGradeupRecipeRes>();
            LearnedNormalSkillsAsset = new LearnedNormalSkillsAsset();
            GPCourseInfoAsset = new GPCourseInfoAsset();
            SecretAbilitiesAsset = new SecretAbilityAsset();
            QuestAssets = new QuestAsset();
            JobValueShopAsset = new List<(JobId, CDataJobValueShopItem)>();
            CostExpScalingAsset = new CostExpScalingAsset();
            SpecialShopAsset = new SpecialShopAsset();
            BitterblackMazeAsset = new BitterblackMazeAsset();
            QuestDropItemAsset = new QuestDropItemAsset();
            RecruitmentBoardCategoryAsset = new RecruitmentBoardCategoryAsset();
            EventDropsAsset = new EventDropsAsset();
            BonusDungeonAsset = new BonusDungeonAsset();
            ClanShopAsset = new Dictionary<uint, ClanShopAsset>();
            EpitaphRoadAssets = new EpitaphRoadAsset();
            EpitaphTrialAssets = new EpitaphTrialAsset();
            PawnCraftSkillCostRateAsset = new();
            PawnCraftSkillSpeedRateAsset = new();
            PawnCraftMasterLegendAsset = new();
            LoadingInfoAsset = new();
            AreaRankSpotInfoAsset = new();
            AreaRankSupplyAsset = new();
            AreaRankRequirementAsset = new();
        }

        public Dictionary<ErrorCode, ClientErrorCode> ClientErrorCodes { get; private set; }
        public Dictionary<uint, ClientItemInfo> ClientItemInfos { get; private set; } // May be incorrect, or incomplete
        public Dictionary<uint, NamedParam> NamedParamAsset { get; private set; }
        public EnemySpawnAsset EnemySpawnAsset { get; private set; }
        public Dictionary<(StageLayoutId, uint), List<GatheringItem>> GatheringItems { get; private set; }
        public List<ServerInfo> ServerList { get; private set; }
        public List<MyPawnCsv> MyPawnAsset { get; private set; }
        public List<MyRoomCsv> MyRoomAsset { get; private set; }
        public List<ArisenCsv> ArisenAsset { get; private set; }
        public List<PawnStartGearCsv> PawnStartGearAsset { get; private set; }
        public List<CDataCharacterItemSlotInfo> StorageAsset { get; private set; }
        public List<Tuple<StorageType, uint, Item>> StorageItemAsset { get; private set; }
        public List<Shop> ShopAsset { get; private set; }
        public List<WarpPoint> WarpPoints { get; private set; }
        public List<S2CCraftRecipeGetCraftRecipeRes> CraftingRecipesAsset { get; private set; }
        public List<S2CCraftRecipeGetCraftGradeupRecipeRes> CraftingGradeUpRecipesAsset { get; private set; }
        public LearnedNormalSkillsAsset LearnedNormalSkillsAsset { get; set; }
        public GPCourseInfoAsset GPCourseInfoAsset { get; private set; }
        public SecretAbilityAsset SecretAbilitiesAsset { get; private set; }
        public CostExpScalingAsset CostExpScalingAsset { get; private set; }
        public QuestAsset QuestAssets {  get; set; }
        public List<(JobId, CDataJobValueShopItem)> JobValueShopAsset { get; private set; }
        public List<CDataStampBonusAsset> StampBonusAsset { get; private set; }
        public SpecialShopAsset SpecialShopAsset { get; private set; }
        public List<PawnCraftSkillCostRate> PawnCraftSkillCostRateAsset { get; private set; }
        public List<PawnCraftSkillSpeedRate> PawnCraftSkillSpeedRateAsset { get; private set; }
        public List<CDataRegisteredLegendPawnInfo> PawnCraftMasterLegendAsset { get; private set; }
        public BitterblackMazeAsset BitterblackMazeAsset { get; private set; }
        public QuestDropItemAsset QuestDropItemAsset { get; private set; }
        public RecruitmentBoardCategoryAsset RecruitmentBoardCategoryAsset { get; private set; }
        public EventDropsAsset EventDropsAsset { get; private set; }
        public BonusDungeonAsset BonusDungeonAsset { get; private set; }
        public Dictionary<uint, ClanShopAsset> ClanShopAsset { get; private set; }
        public EpitaphRoadAsset EpitaphRoadAssets { get; private set; }
        public EpitaphTrialAsset EpitaphTrialAssets { get; private set; }
        public List<CDataLoadingInfoSchedule> LoadingInfoAsset { get; private set; }
        public List<AreaRankSpotInfo> AreaRankSpotInfoAsset { get; private set; }
        public List<AreaRankSupply> AreaRankSupplyAsset { get; private set; }
        public List<AreaRankRequirement> AreaRankRequirementAsset { get; private set; }

        public void Initialize()
        {
            RegisterAsset(value => ClientErrorCodes = value, ClientErrorCodesKey, new ClientErrorCodeAssetDeserializer());
            RegisterAsset(value => ClientItemInfos = value.ToDictionary(key => key.ItemId, val => val), ItemListKey, new ClientItemInfoCsv());
            RegisterAsset(value => NamedParamAsset = value, NamedParamsKey, new NamedParamAssetDeserializer());
            RegisterAsset(value => EnemySpawnAsset = value, EnemySpawnsKey, new EnemySpawnAssetDeserializer(this.NamedParamAsset));
            RegisterAsset(value => GatheringItems = value, GatheringItemsKey, new GatheringItemCsv());
            RegisterAsset(value => MyPawnAsset = value, MyPawnAssetKey, new MyPawnCsvReader());
            RegisterAsset(value => MyRoomAsset = value, MyRoomAssetKey, new MyRoomCsvReader());
            RegisterAsset(value => ArisenAsset = value, ArisenAssetKey, new ArisenCsvReader());
            RegisterAsset(value => PawnStartGearAsset = value, PawnStartGearKey, new PawnStartGearCsvReader());
            RegisterAsset(value => ServerList = value, ServerListKey, new GameServerListInfoCsv());
            RegisterAsset(value => StorageAsset = value, StorageKey, new StorageCsv());
            RegisterAsset(value => StorageItemAsset = value, StorageItemKey, new StorageItemCsv());
            RegisterAsset(value => ShopAsset = value, ShopKey, new JsonReaderWriter<List<Shop>>());
            RegisterAsset(value => WarpPoints = value, WarpPointsKey, new WarpPointCsv());
            RegisterAsset(value => CraftingRecipesAsset = value, CraftingRecipesKey, new JsonReaderWriter<List<S2CCraftRecipeGetCraftRecipeRes>>());
            RegisterAsset(value => CraftingGradeUpRecipesAsset = value, CraftingRecipesGradeUpKey, new JsonReaderWriter<List<S2CCraftRecipeGetCraftGradeupRecipeRes>>());
            RegisterAsset(value => LearnedNormalSkillsAsset = value, LearnedNormalSkillsKey, new LearnedNormalSkillsDeserializer());
            RegisterAsset(value => GPCourseInfoAsset = value, GPCourseInfoKey, new GPCourseInfoDeserializer());
            RegisterAsset(value => SecretAbilitiesAsset = value, SecretAbilityKey, new SecretAbilityDeserializer());
            RegisterAsset(value => JobValueShopAsset = value, JobValueShopKey, new JobValueShopCsv());
            RegisterAsset(value => StampBonusAsset = value, StampBonusKey, new StampBonusCsv());
            RegisterAsset(value => CostExpScalingAsset = value, CostExpScalingInfoKey, new CostExpScalingAssetDeserializer());
            RegisterAsset(value => SpecialShopAsset = value, SpecialShopKey, new SpecialShopDeserializer());
            RegisterAsset(value => BitterblackMazeAsset = value, BitterblackMazeKey, new BitterblackMazeAssetDeserializer());
            RegisterAsset(value => QuestDropItemAsset = value, QuestDropItemsKey, new QuestDropAssetDeserializer());
            RegisterAsset(value => RecruitmentBoardCategoryAsset = value, RecruitmentBoardCategoryKey, new RecruitmentBoardCategoryDeserializer());
            RegisterAsset(value => EventDropsAsset = value, EventDropsKey, new EventDropAssetDeserializer());
            RegisterAsset(value => BonusDungeonAsset = value, BonusDungeonKey, new BonusDungeonAssetDeserializer());
            RegisterAsset(value => ClanShopAsset = value.ToDictionary(key => key.LineupId, value => value), ClanShopKey, new ClanShopCsv());
            RegisterAsset(value => EpitaphRoadAssets = value, EpitaphRoadKey, new EpitaphRoadAssertDeserializer());
            RegisterAsset(value => PawnCraftSkillCostRateAsset = value, PawnCraftSkillCostRateKey, new PawnCraftSkillCostRateCsv());
            RegisterAsset(value => PawnCraftSkillSpeedRateAsset = value, PawnCraftSkillSpeedRateKey, new PawnCraftSkillSpeedRateCsv());
            RegisterAsset(value => PawnCraftMasterLegendAsset = value, PawnCraftMasterLegendKey, new PawnCraftMasterLegendDeserializer());
            RegisterAsset(value => LoadingInfoAsset = value, LoadingInfoKey, new LoadingInfoDeserializer());
            RegisterAsset(value => AreaRankSpotInfoAsset = value, AreaRankSpotInfoKey, new AreaRankSpotInfoCsv());
            RegisterAsset(value => AreaRankSupplyAsset = value, AreaRankSupplyKey, new AreaRankSupplyDeserializer());
            RegisterAsset(value => AreaRankRequirementAsset = value, AreaRankRequirementKey, new AreaRankRequirementDeserializer());

            // This must be set before calling QuestAssertDeserializer and EpitaphTrialAssertDeserializer
            var commonEnemyDeserializer = new AssetCommonDeserializer(this.NamedParamAsset);

            var questAssetDeserializer = new QuestAssetDeserializer(commonEnemyDeserializer, QuestDropItemAsset);
            questAssetDeserializer.LoadQuestsFromDirectory(Path.Combine(_directory.FullName, QuestAssestKey), QuestAssets);

            var epitaphTrialDeserializer = new EpitaphTrialAssetDeserializer(commonEnemyDeserializer, QuestDropItemAsset);
            epitaphTrialDeserializer.LoadTrialsFromDirectory(Path.Combine(_directory.FullName, EpitaphAssestKey), EpitaphTrialAssets);
        }

        private void RegisterAsset<T>(Action<T> onLoadAction, string key, IAssetDeserializer<T> readerWriter)
        {
            try
            {
                Load(onLoadAction, key, readerWriter);
            }
            catch (IOException e)
            {
                Logger.Error($"Could not load '{key}'");
                Logger.Exception(e);
            }

            RegisterFileSystemWatcher(onLoadAction, key, readerWriter);
        }

        private void Load<T>(Action<T> onLoadAction, string key, IAssetDeserializer<T> readerWriter)
        {
            string path = Path.Combine(_directory.FullName, key);

            FileSystemInfo info = File.GetAttributes(path).HasFlag(FileAttributes.Directory) ? new DirectoryInfo(path) : new FileInfo(path);
            if (!info.Exists)
            {
                throw new IOException($"The file or directory '{key}' does not exist");
            }

            if (info is DirectoryInfo)
            {
                foreach (var file in ((DirectoryInfo)info).EnumerateFiles())
                {
                    T asset = readerWriter.ReadPath(file.FullName);
                    onLoadAction.Invoke(asset);
                    OnAssetChanged(file.FullName, asset);
                }
            }
            else
            {
                T asset = readerWriter.ReadPath(info.FullName);
                onLoadAction.Invoke(asset);
                OnAssetChanged(key, asset);
            }
        }

        private void RegisterFileSystemWatcher<T>(Action<T> onLoadAction, string key, IAssetDeserializer<T> readerWriter)
        {
            if (_fileSystemWatchers.ContainsKey(key))
            {
                return;
            }

            FileSystemWatcher watcher = new FileSystemWatcher(_directory.FullName, key);
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Changed += (object sender, FileSystemEventArgs e) =>
            {
                // Try reloading file
                int attempts = 0;
                while (true)
                {
                    try
                    {
                        Load(onLoadAction, key, readerWriter);
                        break;
                    }
                    catch (IOException ex)
                    {
                        // File isn't ready yet, so we need to keep on waiting until it is.
                        attempts++;
                        Logger.Info($"Attempt {attempts} to reload {e.FullPath} unsuccessful.");
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
