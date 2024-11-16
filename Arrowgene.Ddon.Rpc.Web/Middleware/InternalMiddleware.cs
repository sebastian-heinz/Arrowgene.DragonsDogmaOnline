using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Rpc.Web.Middleware
{
    public class InternalMiddleware : IWebMiddleware
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(InternalMiddleware));
        private readonly DdonGameServer _gameServer;
        private readonly HashSet<string> _routes;

        public InternalMiddleware(DdonGameServer gameServer)
        {
            _gameServer = gameServer;
            _routes = new HashSet<string>();
        }

        public void Require(string route)
        {
            _routes.Add(route);
        }

        public async Task<WebResponse> Handle(WebRequest request, WebMiddlewareDelegate next)
        {
            if (!_routes.Contains(request.Path))
            {
                // Don't intercept request if the request path isn't registered in the middleware
                return await next(request);
            }

            string authHeader = request.Header.Get("authorization");
            if (authHeader == null)
            {
                Logger.Error("Attempted to access internal protected route with no Authorization header");
                WebResponse response = new WebResponse();
                response.StatusCode = 401;
                await response.WriteAsync("Attempted to access internal protected route with no Authorization header");
                return response;
            }

            if (!authHeader.StartsWith("Internal "))
            {
                Logger.Error("Attempted to access internal protected route.");
                WebResponse response = new WebResponse();
                response.StatusCode = 401;
                await response.WriteAsync("Attempted to access internal protected route with an invalid Authorization method.");
                return response;
            }

            string[] idAndToken = authHeader.Substring("Internal ".Length).Split(":");
            if (idAndToken.Length != 2)
            {
                Logger.Error("Attempted to access auth protected route with an invalid Internal auth header.");
                WebResponse response = new WebResponse();
                response.StatusCode = 401;
                await response.WriteAsync("Attempted to access auth protected route with an invalid Internal auth header.");
                return response;
            }

            ushort id = ushort.Parse(idAndToken[0]);
            string token = idAndToken[1];

            if (!_gameServer.RpcManager.Auth(id, token))
            {
                Logger.Error($"Attempted to access internal protected route as with incorrect credentials {id}:{token}");
                WebResponse response = new WebResponse();
                response.StatusCode = 403;
                await response.WriteAsync($"Attempted to access internal protected route as without correct credentials.");
                return response;
            }

            return await next(request);
        }
    }
}
