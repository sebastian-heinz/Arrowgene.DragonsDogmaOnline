using System.Threading.Tasks;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.WebServer;

namespace Arrowgene.Ddon.Rpc.Web.Route
{
    public class SpawnRoute : RpcWebRoute
    {
        public override string Route => "/rpc/spawn";

        public SpawnRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override async Task<WebResponse> Get(WebRequest request)
        {
            WebResponse response = new WebResponse();
            response.StatusCode = 200;
            await response.WriteAsync("Welcome - Index Page!");
            return response;
        }
    }
}
