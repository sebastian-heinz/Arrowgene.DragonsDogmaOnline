using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Threading;

namespace Arrowgene.Ddon.Shared
{
    public class AssetRepository
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AssetRepository));

        public event EventHandler UpdatedEnemySpawnsEvent;

        private readonly DirectoryInfo _directory;
        private Dictionary<string, FileSystemWatcher> _fileSystemWatchers;
        private List<EnemySpawn> _localEnemySpawns;
        private List<EnemySpawn> _spreadsheetEnemySpawns;
        private EnemySpawnCsvReader _enemySpawnCsvReader;

        public AssetRepository(string folder)
        {
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                Logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            _fileSystemWatchers = new Dictionary<string, FileSystemWatcher>();

            ClientErrorCodes = new List<ClientErrorCode>();
            EnemySpawns = new List<EnemySpawn>();

            _localEnemySpawns = new List<EnemySpawn>();
            _spreadsheetEnemySpawns = new List<EnemySpawn>();

            _enemySpawnCsvReader = new EnemySpawnCsvReader();
            _enemySpawnCsvReader.EnforceCRLF = false;

            MyPawnAsset = new List<MyPawnCsv>();
            MyRoomAsset = new List<MyRoomCsv>();
            ArisenAsset = new List<ArisenCsv>();
        }

        public List<ClientErrorCode> ClientErrorCodes { get; }
        public IEnumerable<EnemySpawn> EnemySpawns { get; private set; }
        public List<MyPawnCsv> MyPawnAsset { get; }
        public List<MyRoomCsv> MyRoomAsset { get; }
        public List<ArisenCsv> ArisenAsset { get; }

        public void Initialize()
        {
            ClientErrorCodes.Clear();
            RegisterAsset(ClientErrorCodes, "ClientErrorCodes.csv", new ClientErrorCodeCsvReader());
            RegisterAsset(_localEnemySpawns, "EnemySpawn.csv", _enemySpawnCsvReader, _ => UpdateEnemySets());
            RegisterSpreadsheet(_spreadsheetEnemySpawns, "1KmwWymqdMGtbRUqu9GvSi_97o-rBj5DJVn2hk7tvs-A", _enemySpawnCsvReader);
            MyPawnAsset.Clear();
            RegisterAsset(MyPawnAsset, "MyPawn.csv", new MyPawnCsvReader());
            MyRoomAsset.Clear();
            RegisterAsset(MyRoomAsset, "MyRoom.csv", new MyRoomCsvReader());
            ArisenAsset.Clear();
            RegisterAsset(ArisenAsset, "Arisen.csv", new ArisenCsvReader());
        }

        private void RegisterAsset<T>(List<T> list, string key, CsvReader<T> reader, Action<List<T>> afterLoadAction = null)
        {
            Load(list, key, reader);
            if(afterLoadAction != null)
            {
                afterLoadAction.Invoke(list);
            }

            if(!_fileSystemWatchers.ContainsKey(key))
            {
                // Listen for changes in the enemy csv file and update the spawns
                FileSystemWatcher watcher = new FileSystemWatcher(_directory.FullName, key);
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += (object sender, FileSystemEventArgs e) => 
                {
                    Logger.Debug($"Reloading assets from file '{e.FullPath}'");
                    list.Clear();

                    // Try reloading file
                    int attempts = 0;
                    while(true)
                    {
                        try
                        {
                            Load(list, e.FullPath, reader);
                            break;
                        }
                        catch (IOException ex)
                        {
                            // File isn't ready yet, so we need to keep on waiting until it is.
                            attempts++;
                            Logger.Write(LogLevel.Error, $"Failed to reload {e.FullPath}. {attempts} attempts", ex);

                            if(attempts > 10)
                            {
                                Logger.Write(LogLevel.Error, $"Failed to reload {e.FullPath} after {attempts} attempts. Giving up.", ex);
                                break;
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    
                    if(afterLoadAction != null)
                    {
                        afterLoadAction.Invoke(list);
                    }
                };
                watcher.EnableRaisingEvents = true;
                _fileSystemWatchers.Add(key, watcher);
            }
        }

        private void RegisterSpreadsheet<T>(List<T> list, string key, CsvReader<T> reader, Action<List<T>> afterLoadAction = null)
        {
            LoadSpreadsheet(list, key, reader);

            if(afterLoadAction != null)
            {
                afterLoadAction.Invoke(list);
            }
        }

        private void LoadSpreadsheet<T>(List<T> list, string key, CsvReader<T> reader)
        {
            // TODO temporary test google doc csv
            return;
            string url = $"https://docs.google.com/spreadsheets/d/{key}/export?format=csv";
            using var client = new WebClient();
            string csv = client.DownloadString(url);
            list.AddRange(reader.ReadString(csv));
        }

        private void Load<T>(List<T> list, string fileName, CsvReader<T> reader)
        {
            string path = Path.Combine(_directory.FullName, fileName);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                Logger.Error($"Could not load '{fileName}', file does not exist");
            }

            list.AddRange(reader.ReadPath(file.FullName));
        }

        private void Load<T>(Dictionary<int, T> dictionary, string fileName, CsvReader<T> reader)
        {
            List<T> items = new List<T>();
            Load(items, fileName, reader);
            foreach (T item in items)
            {
                if (!(item is IAsset repositoryItem))
                {
                    continue;
                }

                if (dictionary.ContainsKey(repositoryItem.Id))
                {
                    Logger.Error($"Key: '{repositoryItem.Id}' already exists, skipping");
                    continue;
                }

                dictionary.Add(repositoryItem.Id, item);
            }
        }

        private void UpdateEnemySets()
        {
            this.EnemySpawns = _localEnemySpawns.Concat(_spreadsheetEnemySpawns);

            if(UpdatedEnemySpawnsEvent != null)
            {
                UpdatedEnemySpawnsEvent(this, EventArgs.Empty);
            }
        }
    }
}
