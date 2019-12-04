using System.Threading.Tasks;
using Arrowgene.Services.Logging;

namespace Ddo.Server.Web.Middleware
{
    public abstract class WebMiddleware : IWebMiddleware
    {
        protected ILogger Logger => LogProvider.Instance.GetLogger(this);
        public abstract Task<WebResponse> Handle(WebRequest request, WebMiddlewareDelegate next);
    }
}
