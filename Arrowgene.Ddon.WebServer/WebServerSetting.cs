using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Network;
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
            WebSetting.HttpPorts = new List<ushort>() {52099};
            WebSetting.HttpsEnabled = false;
            WebSetting.HttpsPort = 443;
            WebSetting.HttpsCertPath = "";
            WebSetting.HttpsCertPw = "";
        }

        public WebServerSetting(WebServerSetting webServerSetting)
        {
            WebSetting = new WebSetting(webServerSetting.WebSetting);
        }

        [DataMember(Order = 1)] public WebSetting WebSetting { get; set; }
    }
}
