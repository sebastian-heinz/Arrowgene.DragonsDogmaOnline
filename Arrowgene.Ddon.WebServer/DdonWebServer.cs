using System.Threading.Tasks;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Server;
using Arrowgene.WebServer.Server.Kestrel;
using Arrowgene.WebServer.WebMiddleware;
using Microsoft.Extensions.FileProviders;

namespace Arrowgene.Ddon.WebServer
{
    public class DdonWebServer
    {
        private readonly WebService _webService;
        private readonly WebServerSetting _setting;

        public DdonWebServer(WebServerSetting setting)
        {
            _setting = setting;

            IWebServerCore core = new KestrelWebServer(_setting.WebSetting);
            _webService = new WebService(core);

            IFileProvider webFileProvider = new PhysicalFileProvider(_setting.WebSetting.WebFolder);
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
