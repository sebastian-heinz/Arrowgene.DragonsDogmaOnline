using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Middleware;
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
        private bool _running;

        public DdonWebServer(WebServerSetting setting, IDatabase database)
        {
            _setting = setting;
            _running = false;

            IWebServerCore core = new KestrelWebServer(_setting.WebSetting);
            _webService = new WebService(core);

            Logger.Info($"Serving Directory: {_setting.WebSetting.WebFolder}");
            var staticFile = new StaticFileMiddleware(new PhysicalFileProvider(_setting.WebSetting.WebFolder));
            foreach (string servingFile in staticFile.GetServingFilesUrl(_setting.PublicWebEndPoint))
            {
                Logger.Info(servingFile);
            }

            AddMiddleware(staticFile);

            AddRoute(new IndexRoute());
            AddRoute(new AccountRoute(database));
        }

        public void AddMiddleware(IWebMiddleware middleware)
        {
            _webService.AddMiddleware(middleware);
            if (_running)
            {
                Logger.Info($"Registered new middleware `{middleware.GetType().Name}`");
            }
        }

        public void AddRoute(IWebRoute route)
        {
            _webService.AddRoute(route);
            if (_running)
            {
                Logger.Info($"Registered new route `{route.Route}`, now serving endpoints:");
                foreach (WebRequestMethod method in route.GetMethods())
                {
                    Logger.Info($"[{method}] {_setting.PublicWebEndPoint.GetUrl()}{route.Route}");
                }
            }
        }

        public async Task Start()
        {
            Logger.Info($"Serving Routes:");
            foreach (string servingRoute in _webService.GetServingRoutes(_setting.PublicWebEndPoint))
            {
                Logger.Info(servingRoute);
            }

            _running = true;

            foreach (WebEndPoint webEndPoint in _setting.WebSetting.WebEndpoints)
            {
                Logger.Info($"Listening: {webEndPoint.IpAddress}:{webEndPoint.Port}");
            }

            await _webService.Start();
            // only returns once webserver stopped

            _running = false;
        }

        public async Task Stop()
        {
            _running = false;
            await _webService.Stop();
        }
    }
}
