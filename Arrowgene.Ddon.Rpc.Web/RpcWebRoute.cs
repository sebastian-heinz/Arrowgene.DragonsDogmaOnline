using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.WebServer.Route;

namespace Arrowgene.Ddon.Rpc.Web
{
    public abstract class RpcWebRoute : WebRoute, IRpcExecuter
    {
        private readonly IRpcExecuter _executer;

        public RpcWebRoute(IRpcExecuter executer)
        {
            _executer = executer;
        }

        public RpcCommandResult Execute(IRpcCommand command)
        {
           return _executer.Execute(command);
        }
    }
}
