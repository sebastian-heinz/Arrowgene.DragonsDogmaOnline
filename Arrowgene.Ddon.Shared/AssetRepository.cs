using System.Collections.Generic;
using System.IO;
using System.Net;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared
{
    public class AssetRepository
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AssetRepository));

        private readonly DirectoryInfo _directory;

        public AssetRepository(string folder)
        {
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                Logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            ClientErrorCodes = new List<ClientErrorCode>();
            EnemySpawns = new List<EnemySpawn>();
        }

        public List<ClientErrorCode> ClientErrorCodes { get; }
        public List<EnemySpawn> EnemySpawns { get; }


        public void Initialize()
        {
            ClientErrorCodes.Clear();
            Load(ClientErrorCodes, "ClientErrorCodes.csv", new ClientErrorCodeCsvReader());
            Load(EnemySpawns, "EnemySpawn.csv", new EnemySpawnCsvReader());
            LoadSpreadsheet(EnemySpawns, "1KmwWymqdMGtbRUqu9GvSi_97o-rBj5DJVn2hk7tvs-A", new EnemySpawnCsvReader());
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
    }
}
