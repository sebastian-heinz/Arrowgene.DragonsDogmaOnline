using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Arrowgene.Ddo.Shared;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Server;
using Arrowgene.WebServer.Server.Kestrel;
using Arrowgene.WebServer.WebMiddleware;
using Microsoft.Extensions.FileProviders;

namespace Arrowgene.Ddo.WebServer
{
    public class DdoWebServer
    {
        private readonly WebService _webService;

        public DdoWebServer()
        {
            WebSetting setting = new WebSetting();
            setting.ServerHeader = "";
            setting.WebFolder = Path.Combine(Util.ExecutingDirectory(), "Files/www");
            setting.HttpPorts = new List<ushort>() {80};
            setting.HttpsEnabled = false;
            setting.HttpsPort = 443;
            setting.HttpsCertPath = "";
            setting.HttpsCertPw = "";
            IWebServerCore core = new KestrelWebServer(setting);
            _webService = new WebService(core);

            IFileProvider webFileProvider = new PhysicalFileProvider(setting.WebFolder);
            _webService.AddMiddleware(new StaticFileMiddleware("", webFileProvider));

            _webService.AddRoute(new IndexRoute());
        }

        public async Task Start()
        {
            await _webService.Start();
        }

        public async Task Stop()
        {
            await _webService.Stop();
        }
    }
}
