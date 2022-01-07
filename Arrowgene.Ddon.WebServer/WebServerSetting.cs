using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.WebServer
{
    [DataContract]
    public class WebServerSetting
    {
        public WebServerSetting()
        {
            ServerHeader = null;
            WebFolder = Path.Combine(Util.ExecutingDirectory(), "Files/www");
            HttpPort = 80;
            HttpsEnabled = false;
            HttpsPort = 443;
            HttpsCertPath = Path.Combine(Util.ExecutingDirectory(), "Files/ddon.pfx");
        }

        public WebServerSetting(WebServerSetting webSetting)
        {
            ServerHeader = webSetting.ServerHeader;
            WebFolder = webSetting.WebFolder;
            HttpPort = webSetting.HttpPort;
            HttpsEnabled = webSetting.HttpsEnabled;
            HttpsPort = webSetting.HttpsPort;
            HttpsCertPath = webSetting.HttpsCertPath;
        }

        [DataMember(Order = 1)] public string ServerHeader { get; set; }
        [DataMember(Order = 2)] public short HttpPort { get; set; }
        [DataMember(Order = 3)] public bool HttpsEnabled { get; set; }
        [DataMember(Order = 4)] public short HttpsPort { get; set; }
        [DataMember(Order = 5)] public string HttpsCertPath { get; set; }
        [DataMember(Order = 6)] public string WebFolder { get; set; }
    }
}
