using System.Net;
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
            Logger.Info($"Serving Directory: {_setting.WebSetting.WebFolder}");
            _webService.AddMiddleware(new StaticFileMiddleware("", webFileProvider));

            AddRoute(new IndexRoute());
            AddRoute(new AccountRoute(_database));
        }

        public void AddRoute(IWebRoute route)
        {
            Logger.Info($"Registered Route: {route.Route}");
            _webService.AddRoute(route);
        }

        public async Task Start()
        {
            // TODO update `Arrowgene.WebServer` to expose bound ports after startup, expose route METHOD, 
            // TODO static file server -> enumerate files and folders served, remove _root
            // current implementations does not allow to specify interface
            // uses IPAddress.IPv6Any or IPAddress.Any
            foreach (uint port in _setting.WebSetting.HttpPorts)
            {
                Logger.Info($"Listening: {IPAddress.Any}:{port}");
                // Logger.Info($"Listening: {IPAddress.IPv6Any}:{port}");
            }

            if (_setting.WebSetting.HttpsEnabled)
            {
                Logger.Info($"Listening: {IPAddress.Any}:{_setting.WebSetting.HttpsPort}");
                // Logger.Info($"Listening: {IPAddress.IPv6Any}:{_setting.WebSetting.HttpsPort}");
            }

            await _webService.Start();
            // only returns once webserver stopped
        }

        public async Task Stop()
        {
            await _webService.Stop();
        }
    }
}
