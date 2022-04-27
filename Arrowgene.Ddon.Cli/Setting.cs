using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.WebServer;

namespace Arrowgene.Ddon.Cli
{
    [DataContract]
    public class Setting
    {
        public static Setting Load(string filePath)
        {
            FileInfo f = new FileInfo(filePath);
            if (!f.Exists)
            {
                return null;
            }

            string json = File.ReadAllText(f.FullName);
            Setting setting = JsonContractSerializer.Deserialize<Setting>(json);
            return setting;
        }

        public void Save(string filePath)
        {
            string json = JsonContractSerializer.Serialize(this);
            File.WriteAllText(filePath, json);
        }

        [DataMember(Order = 10)] public WebServerSetting WebServerSetting { get; set; }
        [DataMember(Order = 20)] public GameServerSetting GameServerSetting { get; set; }
        [DataMember(Order = 30)] public LoginServerSetting LoginServerSetting { get; set; }
        [DataMember(Order = 40)] public DatabaseSetting DatabaseSetting { get; set; }
        [DataMember(Order = 50)] public string AssetPath { get; set; }

        public Setting()
        {
            WebServerSetting = new WebServerSetting();
            GameServerSetting = new GameServerSetting();
            LoginServerSetting = new LoginServerSetting();
            DatabaseSetting = new DatabaseSetting();
            AssetPath = Path.Combine(Util.ExecutingDirectory(), "Files/Assets");
        }

        public Setting(Setting setting)
        {
            WebServerSetting = new WebServerSetting(setting.WebServerSetting);
            GameServerSetting = new GameServerSetting(setting.GameServerSetting);
            LoginServerSetting = new LoginServerSetting(setting.LoginServerSetting);
            DatabaseSetting = new DatabaseSetting(setting.DatabaseSetting);
            AssetPath = setting.AssetPath;
        }
    }
}
