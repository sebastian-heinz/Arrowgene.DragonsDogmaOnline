using System;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.WebServer;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private Setting _setting;
        private DdonLoginServer _loginServer;
        private DdonGameServer _gameServer;
        private DdonWebServer _webServer;

        public string Key => "server";

        public string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";

        public CommandResultType Run(CommandParameter parameter)
        {
            if (_setting == null)
            {
                _setting = new Setting();
            }
            
            if (_loginServer == null)
            {
                _loginServer = new DdonLoginServer(_setting.LoginServerSetting);
            }

            if (_webServer == null)
            {
                _webServer = new DdonWebServer(_setting.WebServerSetting);
            }

            if (_gameServer == null)
            {
                _gameServer = new DdonGameServer(_setting.GameServerSetting);
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
