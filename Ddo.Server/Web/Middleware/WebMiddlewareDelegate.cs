using System.Threading.Tasks;

namespace Ddo.Server.Web.Middleware
{
    public delegate Task<WebResponse> WebMiddlewareDelegate(WebRequest request);
}
