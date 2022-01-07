using System.Threading.Tasks;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Route;

namespace Arrowgene.Ddon.WebServer
{
    public class IndexRoute : WebRoute
    {
        public override string Route => "/";
        
        public override async Task<WebResponse> Get(WebRequest request)
        {
            WebResponse response = new WebResponse();
            response.StatusCode = 200;
            await response.WriteAsync("Welcome - Index Page!");
            return response;
        }
    }
}
