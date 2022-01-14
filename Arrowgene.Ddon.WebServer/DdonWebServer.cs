using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
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
        private readonly IDatabase _database;

        public DdonWebServer(WebServerSetting setting)
        {
            _setting = setting;

            _database = DdonDatabaseBuilder.Build(_setting.DatabaseSetting);

            IWebServerCore core = new KestrelWebServer(_setting.WebSetting);
            _webService = new WebService(core);

            IFileProvider webFileProvider = new PhysicalFileProvider(_setting.WebSetting.WebFolder);
            _webService.AddMiddleware(new StaticFileMiddleware("", webFileProvider));

            _webService.AddRoute(new IndexRoute());
            _webService.AddRoute(new AccountRoute(_database));
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
