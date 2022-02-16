using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Route;
using Arrowgene.WebServer.Server;
using Arrowgene.WebServer.Server.Kestrel;
using Arrowgene.WebServer.WebMiddleware;
using Microsoft.Extensions.FileProviders;

namespace Arrowgene.Ddon.WebServer
{
    public class DdonWebServer
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonWebServer));

        private readonly WebService _webService;
        private readonly WebServerSetting _setting;
        private readonly IDatabase _database;

        public DdonWebServer(WebServerSetting setting, IDatabase database)
        {
            _setting = setting;
            _database = database;

            IWebServerCore core = new KestrelWebServer(_setting.WebSetting);
            _webService = new WebService(core);

            IFileProvider webFileProvider = new PhysicalFileProvider(_setting.WebSetting.WebFolder);
            _webService.AddMiddleware(new StaticFileMiddleware("", webFileProvider));

            _webService.AddRoute(new IndexRoute());
            _webService.AddRoute(new AccountRoute(_database));
        }

        public void AddRoute(IWebRoute route) => _webService.AddRoute(route);

        public async Task Start()
        {
            await _webService.Start();
            Logger.Info($"Running Web Server http://localhost/web/index.html");
        }

        public async Task Stop()
        {
            await _webService.Stop();
        }
    }
}
