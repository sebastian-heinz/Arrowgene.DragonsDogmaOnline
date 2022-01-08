using System.Runtime.Serialization;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.WebServer;

namespace Arrowgene.Ddon.Cli
{
    [DataContract]
    public class Setting
    {
        [DataMember(Order = 10)] public WebServerSetting WebServerSetting { get; set; }
        [DataMember(Order = 11)] public LoginServerSetting GameServerSetting { get; set; }
        [DataMember(Order = 12)] public DatabaseSetting DatabaseSetting { get; set; }

        public Setting()
        {
            WebServerSetting = new WebServerSetting();
            GameServerSetting = new LoginServerSetting();
            DatabaseSetting = new DatabaseSetting();
        }

        public Setting(Setting setting)
        {
            setting.WebServerSetting = new WebServerSetting(setting.WebServerSetting);
            setting.GameServerSetting = new LoginServerSetting(setting.GameServerSetting);
            setting.DatabaseSetting = new DatabaseSetting(setting.DatabaseSetting);
        }
    }
}
