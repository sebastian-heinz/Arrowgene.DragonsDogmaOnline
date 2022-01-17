using System;
using System.IO;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.WebServer;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ServerCommand));


        private Setting _setting;
        private IServerProvider _provider;
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
                string settingPath = Path.Combine(Util.ExecutingDirectory(), "Arrowgene.Dd.json");
                _setting = Setting.Load(settingPath);
                if (_setting == null)
                {
                    _setting = new Setting();
                    _setting.Save(settingPath);
                    Logger.Info($"Created new settings and saved to:{settingPath}");
                }
                else
                {
                    Logger.Info($"Loaded settings from:{settingPath}");
                }
            }

            if (_provider == null)
            {
                _provider = new DefaultServerProvider(_setting.AssetPath);
            }

            if (_loginServer == null)
            {
                _loginServer = new DdonLoginServer(_setting.LoginServerSetting, _provider);
            }

            if (_webServer == null)
            {
                _webServer = new DdonWebServer(_setting.WebServerSetting);
            }

            if (_gameServer == null)
            {
                _gameServer = new DdonGameServer(_setting.GameServerSetting, _provider);
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
