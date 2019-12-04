using System.Threading.Tasks;
using Ddo.Server.Web.Route;

namespace Ddo.Server.Web.Server
{
    /// <summary>
    /// Defines web server
    /// </summary>
    public interface IWebServer
    {
        void SetHandler(IWebServerHandler handler);
        Task Start();
        Task Stop();
    }
}
