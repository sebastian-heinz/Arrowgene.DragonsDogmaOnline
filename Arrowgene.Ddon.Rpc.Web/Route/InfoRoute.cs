using System.Threading.Tasks;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.WebServer;

namespace Arrowgene.Ddon.Rpc.Web.Route;

public class InfoRoute : RpcWebRoute
{
    public override string Route => "/rpc/info";

    public InfoRoute(IRpcExecuter executer) : base(executer)
    {
    }

    public override async Task<WebResponse> Get(WebRequest request)
    {
        WebResponse response = new WebResponse();
        InfoCommand info = new InfoCommand();
        RpcCommandResult result = Executer.Execute(info);
        if (!result.Success)
        {
            response.StatusCode = 500;
            await response.WriteAsync("Error");
            return response;
        }
        response.StatusCode = 200;
        await response.WriteJsonAsync(info.Infos);
        return response;
    }
}
