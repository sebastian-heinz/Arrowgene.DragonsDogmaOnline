using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Services.Logging;
using Ddo.Server.Setting;

namespace Ddo.Server.Web.Route
{
    /// <summary>
    /// Parses routes and calls the route
    /// </summary>
    public class WebRouter : IWebRouter
    {
        private Dictionary<string, IWebRoute> _routes;
        private ILogger _logger;
        private DdoSetting _setting;

        public WebRouter(DdoSetting setting)
        {
            _setting = setting;
            _logger = LogProvider.Instance.GetLogger(this);
            _routes = new Dictionary<string, IWebRoute>();
        }


        /// <summary>
        /// Adds a handler for a specific route.
        /// </summary>
        public void AddRoute(IWebRoute route)
        {
            _routes.Add(route.Route, route);
        }

        /// <summary>
        /// Passes incoming requests to the correct route
        /// </summary>
        public async Task<WebResponse> Route(WebRequest request)
        {
            _logger.Info($"Request: {request}");
            if (request.Path == null)
            {
                _logger.Error($"Request path not set, please check sever request mapping implementation");
                return await WebResponse.InternalServerError();
            }

            if (_routes.ContainsKey(request.Path))
            {
                IWebRoute route = _routes[request.Path];
                Task<WebResponse> responseTask = null;
                switch (request.Method)
                {
                    case WebRequestMethod.Get:
                        responseTask = route.Get(request);
                        break;
                    case WebRequestMethod.Post:
                        responseTask = route.Post(request);
                        break;
                    case WebRequestMethod.Put:
                        responseTask = route.Put(request);
                        break;
                    case WebRequestMethod.Delete:
                        responseTask = route.Delete(request);
                        break;
                }

                if (responseTask == null)
                {
                    _logger.Info($"Request method: {request.Method} not supported for requested path: {request.Path}");
                    return await WebResponse.InternalServerError();
                }

                WebResponse response = await responseTask;
                response.RouteFound = true;
                if (!string.IsNullOrEmpty(_setting.WebSetting.ServerHeader))
                {
                    response.Header.Add("Server", _setting.WebSetting.ServerHeader);
                }

                return response;
            }
            
            return await WebResponse.NotFound();
        }
    }
}
