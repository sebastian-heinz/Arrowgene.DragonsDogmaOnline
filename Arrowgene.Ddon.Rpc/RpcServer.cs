using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Rpc
{
    /// <summary>
    /// Executes predefined commands
    /// </summary>
    public class RpcServer : IRpcExecuter
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(RpcServer));

        protected readonly DdonGameServer _gameServer;

        public RpcServer(DdonGameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public RpcCommandResult Execute(IRpcCommand command)
        {
            RpcCommandResult result = command.Execute(_gameServer);
            if (result.Message.Length > 0)
            {
                Logger.Info(result.ToString());
            }
            return result;
        }
    }
}
