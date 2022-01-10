using System.Runtime.Serialization;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.WebServer;

namespace Arrowgene.Ddon.Cli
{
    [DataContract]
    public class Setting
    {
        [DataMember(Order = 10)] public WebServerSetting WebServerSetting { get; set; }
        [DataMember(Order = 20)] public GameServerSetting GameServerSetting { get; set; }
        [DataMember(Order = 30)] public LoginServerSetting LoginServerSetting { get; set; }
        [DataMember(Order = 40)] public DatabaseSetting DatabaseSetting { get; set; }

        public Setting()
        {
            WebServerSetting = new WebServerSetting();
            GameServerSetting = new GameServerSetting();
            LoginServerSetting = new LoginServerSetting();
            DatabaseSetting = new DatabaseSetting();
        }

        public Setting(Setting setting)
        {
            setting.WebServerSetting = new WebServerSetting(setting.WebServerSetting);
            setting.GameServerSetting = new GameServerSetting(setting.GameServerSetting);
            setting.LoginServerSetting = new LoginServerSetting(setting.LoginServerSetting);
            setting.DatabaseSetting = new DatabaseSetting(setting.DatabaseSetting);
        }
    }
}
