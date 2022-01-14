using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.WebServer;

namespace Arrowgene.Ddon.WebServer
{
    [DataContract]
    public class WebServerSetting
    {
        public WebServerSetting()
        {
            WebSetting = new WebSetting();
            WebSetting.ServerHeader = "";
            WebSetting.WebFolder = Path.Combine(Util.ExecutingDirectory(), "Files/www");
            WebSetting.HttpPorts = new List<ushort>() {80};
            WebSetting.HttpsEnabled = false;
            WebSetting.HttpsPort = 443;
            WebSetting.HttpsCertPath = "";
            WebSetting.HttpsCertPw = "";
            DatabaseSetting = new DatabaseSetting();
        }

        public WebServerSetting(WebServerSetting webServerSetting)
        {
            WebSetting = new WebSetting(webServerSetting.WebSetting);
            DatabaseSetting = new DatabaseSetting(webServerSetting.DatabaseSetting);
        }

        [DataMember(Order = 1)] public WebSetting WebSetting { get; set; }
        [DataMember(Order = 2)] public DatabaseSetting DatabaseSetting { get; set; }
    }
}
