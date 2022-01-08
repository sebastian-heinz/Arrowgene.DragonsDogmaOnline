using System;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.WebServer;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private DdonLoginServer _loginServer;
        private DdonGameServer _gameServer;
        private DdonWebServer _webServer;

        public string Key => "server";

        public string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";

        public CommandResultType Run(CommandParameter parameter)
        {
            if (_loginServer == null)
            {
                LoginServerSetting setting = new LoginServerSetting();
                _loginServer = new DdonLoginServer(setting);
            }

            if (_webServer == null)
            {
                WebServerSetting setting = new WebServerSetting();
                _webServer = new DdonWebServer();
            }

            if (_gameServer == null)
            {
                GameServerSetting setting = new GameServerSetting();
                _gameServer = new DdonGameServer(setting);
            }

            if (parameter.Arguments.Contains("start"))
            {
                _webServer.Start();
                _gameServer.Start();
                _loginServer.Start();
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("stop"))
            {
                _webServer.Stop();
                _gameServer.Stop();
                _loginServer.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
            if (_loginServer != null)
            {
                _loginServer.Stop();
            }

            if (_gameServer != null)
            {
                _gameServer.Stop();
            }

            if (_webServer != null)
            {
                _webServer.Stop();
            }
        }
    }
}
