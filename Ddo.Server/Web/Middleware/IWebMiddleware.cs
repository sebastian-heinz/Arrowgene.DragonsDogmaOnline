using System.Threading.Tasks;

namespace Ddo.Server.Web.Middleware
{
    /// <summary>
    /// Defines a middleware
    /// </summary>
    public interface IWebMiddleware
    {
        Task<WebResponse> Handle(WebRequest request, WebMiddlewareDelegate next);
    }
}
