using System;
using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.WebServer;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli
{
    [DataContract]
    public class Setting
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(Setting));

        [DataMember(Order = 0)] public string LogPath { get; set; }
        [DataMember(Order = 10)] public WebServerSetting WebServerSetting { get; set; }
        [DataMember(Order = 20)] public GameServerSetting GameServerSetting { get; set; }
        [DataMember(Order = 30)] public LoginServerSetting LoginServerSetting { get; set; }
        [DataMember(Order = 40)] public DatabaseSetting DatabaseSetting { get; set; }
        [DataMember(Order = 50)] public string AssetPath { get; set; }

        // Note: constructor is not called during deserialization.
        public Setting()
        {
            LogPath = "Logs";
            WebServerSetting = new WebServerSetting();
            GameServerSetting = new GameServerSetting();
            LoginServerSetting = new LoginServerSetting();
            DatabaseSetting = new DatabaseSetting();
            AssetPath = Path.Combine(Util.ExecutingDirectory(), "Files/Assets");
        }

        // Note: constructor is not called during deserialization.
        public Setting(Setting setting)
        {
            LogPath = setting.LogPath;
            WebServerSetting = new WebServerSetting(setting.WebServerSetting);
            GameServerSetting = new GameServerSetting(setting.GameServerSetting);
            LoginServerSetting = new LoginServerSetting(setting.LoginServerSetting);
            DatabaseSetting = new DatabaseSetting(setting.DatabaseSetting);
            AssetPath = setting.AssetPath;
        }

        // Note: method is called after the object is completely deserialized. Use it instead of the constructror.
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            LogPath ??= "Logs";
            WebServerSetting ??= new WebServerSetting();
            GameServerSetting ??= new GameServerSetting();
            LoginServerSetting ??= new LoginServerSetting();
            DatabaseSetting ??= new DatabaseSetting();
            AssetPath ??= Path.Combine(Util.ExecutingDirectory(), "Files/Assets");
        }

        public static Setting Deserialize(string json)
        {
            Setting settings;
            try
            {
                settings = JsonContractSerializer.Deserialize<Setting>(json);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw;
            }

            return settings;
        }

        public static string Serialize(Setting setting)
        {
            string json = null;
            try
            {
                json = JsonContractSerializer.Serialize(setting);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw;
            }

            return json;
        }

        public static Setting Load(string json)
        {
            Setting setting = Deserialize(json);
            return setting;
        }

        public static Setting LoadFromFile(FileInfo file)
        {
            if (!file.Exists)
            {
                return null;
            }

            return Load(File.ReadAllText(file.FullName));
        }

        public static Setting LoadFromFile(string filePath)
        {
            return LoadFromFile(new FileInfo(filePath));
        }

        public static void Save(string filePath, Setting setting)
        {
            string json = Serialize(setting);
            File.WriteAllText(filePath, json);
        }

        public void Save(string filePath)
        {
            Save(filePath, this);
        }

        public void Save(FileInfo file)
        {
            Save(file.FullName, this);
        }
    }
}
