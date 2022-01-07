using System;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.WebServer;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private DdonGameServer _gameServer;
        private DdonWebServer _webServer;

        public string Key => "server";

        public string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";

        public CommandResultType Run(CommandParameter parameter)
        {
            if (_gameServer == null)
            {
                GameServerSetting setting = new GameServerSetting();
                _gameServer = new DdonGameServer(setting);
            }

            if (_webServer == null)
            {
                WebServerSetting setting = new WebServerSetting();
                _webServer = new DdonWebServer();
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
