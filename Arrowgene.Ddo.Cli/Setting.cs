using System.Runtime.Serialization;
using Arrowgene.Ddo.Database;
using Arrowgene.Ddo.GameServer;
using Arrowgene.Ddo.WebServer;

namespace Arrowgene.Ddo.Cli
{
    [DataContract]
    public class Setting
    {
        [DataMember(Order = 10)] public WebServerSetting WebServerSetting { get; set; }
        [DataMember(Order = 11)] public GameServerSetting GameServerSetting { get; set; }
        [DataMember(Order = 12)] public DatabaseSetting DatabaseSetting { get; set; }
        
        public Setting()
        {
            WebServerSetting = new WebServerSetting();
            GameServerSetting = new GameServerSetting();
            DatabaseSetting = new DatabaseSetting();
        }

        public Setting(Setting setting)
        {
            setting.WebServerSetting = new WebServerSetting(setting.WebServerSetting);
            setting.GameServerSetting = new GameServerSetting(setting.GameServerSetting);
            setting.DatabaseSetting = new DatabaseSetting(setting.DatabaseSetting);
        }
    }
}
