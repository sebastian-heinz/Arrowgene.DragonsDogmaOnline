using System.Threading.Tasks;

namespace Ddo.Server.Web.Route
{
    public interface IWebRouter
    {
        void AddRoute(IWebRoute route);
        Task<WebResponse> Route(WebRequest request);
    }
}
