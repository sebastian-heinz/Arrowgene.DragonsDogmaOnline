using System.Threading.Tasks;

namespace Ddo.Server.Web.Server
{
    public interface IWebServerHandler
    {
        Task<WebResponse> Handle(WebRequest request);
    }
}
