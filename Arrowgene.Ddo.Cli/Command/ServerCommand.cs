using System;
using Arrowgene.Ddo.GameServer;
using Arrowgene.Ddo.WebServer;

namespace Arrowgene.Ddo.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private DdoGameServer _gameServer;
        private DdoWebServer _webServer;

        public string Key => "server";

        public string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";

        public CommandResultType Run(CommandParameter parameter)
        {
            if (_gameServer == null)
            {
                GameServerSetting setting = new GameServerSetting();
                _gameServer = new DdoGameServer(setting);
            }

            if (_webServer == null)
            {
                WebServerSetting setting = new WebServerSetting();
                _webServer = new DdoWebServer();
            }

            if (parameter.Arguments.Contains("start"))
            {
                _webServer.Start();
                _gameServer.Start();
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("stop"))
            {
                _webServer.Stop();
                _gameServer.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }
        
        public void Shutdown()
        {
            if (_gameServer != null)
            {
                _webServer.Stop();
                _gameServer.Stop();
            }
        }
    }
}
