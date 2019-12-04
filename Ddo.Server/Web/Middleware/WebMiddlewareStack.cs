using System;
using System.Threading.Tasks;

namespace Ddo.Server.Web.Middleware
{
    /// <summary>
    /// Implementation of a middleware
    /// </summary>
    public class WebMiddlewareStack
    {
        private WebMiddlewareDelegate _webMiddlewareDelegate;

        public WebMiddlewareStack(WebMiddlewareDelegate kernel)
        {
            _webMiddlewareDelegate = kernel;
        }

        public Task<WebResponse> Start(WebRequest request)
        {
            return _webMiddlewareDelegate(request);
        }

        public void Use(Func<WebMiddlewareDelegate, WebMiddlewareDelegate> middleware)
        {
            _webMiddlewareDelegate = middleware(_webMiddlewareDelegate);
        }
    }
}
