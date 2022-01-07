using System.IO;
using System.Text.Json;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.Cli
{
    public class SettingProvider
    {
        private readonly string _directory;


        public SettingProvider(string directory = null)
        {
            if (Directory.Exists(directory))
            {
                _directory = directory;
            }
            else
            {
                _directory = Util.ExecutingDirectory();
            }
        }

        public string GetSettingsPath(string file)
        {
            return Path.Combine(_directory, file);
        }

        public void Save<T>(T settings, string file)
        {
            string path = GetSettingsPath(file);
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(path, json);
        }

        public T Load<T>(string file)
        {
            T settings;
            string path = GetSettingsPath(file);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                settings = JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                settings = default(T);
            }

            return settings;
        }
    }
}
