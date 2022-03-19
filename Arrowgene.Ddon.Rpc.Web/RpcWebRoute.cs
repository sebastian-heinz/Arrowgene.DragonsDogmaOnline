using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.WebServer.Route;

namespace Arrowgene.Ddon.Rpc.Web
{
    public abstract class RpcWebRoute : WebRoute, IRpcExecuter
    {
        protected readonly IRpcExecuter Executer;

        protected RpcWebRoute(IRpcExecuter executer)
        {
            Executer = executer;
        }

        public RpcCommandResult Execute(IRpcCommand command)
        {
           return Executer.Execute(command);
        }
    }
}
