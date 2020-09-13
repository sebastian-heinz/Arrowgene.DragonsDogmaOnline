using System;
using Arrowgene.Ddo.GameServer;

namespace Arrowgene.Ddo.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private DdoGameServer _server;

        public string Key => "server";

        public string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";

        public CommandResultType Run(CommandParameter parameter)
        {
            if (_server == null)
            {
                GameServerSetting setting = new GameServerSetting();
                _server = new DdoGameServer(setting);
            }

            if (parameter.Arguments.Contains("start"))
            {
                _server.Start();
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("stop"))
            {
                _server.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }
        
        public void Shutdown()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }
    }
}
