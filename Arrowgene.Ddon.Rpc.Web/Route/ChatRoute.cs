using System.Threading.Tasks;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.WebServer;

namespace Arrowgene.Ddon.Rpc.Web.Route
{
    public class ChatRoute : RpcWebRoute
    {
        public override string Route => "/rpc/chat";

        public ChatRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override async Task<WebResponse> Get(WebRequest request)
        {
            WebResponse response = new WebResponse();
            ChatCommand chat = new ChatCommand();
            RpcCommandResult result = Executer.Execute(chat);
            if (!result.Success)
            {
                response.StatusCode = 500;
                await response.WriteAsync("Error");
                return response;
            }
            response.StatusCode = 200;
            await response.WriteJsonAsync(chat.ChatMessageLog);
            return response;
        }

    }
}