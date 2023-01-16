using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.Rpc.Web;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.WebServer;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ServerCommand));

        private Setting _setting;
        private DdonLoginServer _loginServer;
        private DdonGameServer _gameServer;
        private DdonWebServer _webServer;
        private RpcWebServer _rpcWebServer;
        private IDatabase _database;
        private AssetRepository _assetRepository;

        public string Key => "server";

        public string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";


        public CommandResultType Run(CommandParameter parameter)
        {
            Logger.Info("*** NOTICE ***");
            Logger.Info("This software is and always will be free of charge available at:");
            Logger.Info("https://github.com/sebastian-heinz/Arrowgene.DragonsDogmaOnline");
            Logger.Info($"***{Environment.NewLine}");

            Logger.Info("*** WARNING ***");
            Logger.Info("This software listens on ports for incoming requests and processes them,");
            Logger.Info("serves files via http/https protocol and exposes APIs.");
            Logger.Info("All of the above happens with no to limited authentication required.");
            Logger.Info("Please be aware that it may expose or put your system at risk");
            Logger.Info($"***{Environment.NewLine}");

            Logger.Info($"Startup: {DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)}");
            Logger.Info($"OS: {Environment.OSVersion}");
            Logger.Info($"x64 OS: {Environment.Is64BitOperatingSystem}");
            Logger.Info($"Processors: {Environment.ProcessorCount}");
            Logger.Info($".NET Version: {Environment.Version}");
            Logger.Info($"x64 Process: {Environment.Is64BitProcess}");
            Logger.Info($"CurrentDirectory: {Environment.CurrentDirectory}");
            Logger.Info($"ApplicationDirectory: {Util.ExecutingDirectory()}");
            Logger.Info($"RelativeApplicationDirectory: {Util.RelativeExecutingDirectory()}");

            bool isService = parameter.Switches.Contains("--service");

            if (_setting == null)
            {
                string settingPath = Path.Combine(Util.ExecutingDirectory(), "Files/Arrowgene.Ddon.config.json");
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
            
            if (_database == null)
            {
                _database = DdonDatabaseBuilder.Build(_setting.DatabaseSetting);
            }

            if (_assetRepository == null)
            {
                _assetRepository = new AssetRepository(_setting.AssetPath);
                _assetRepository.Initialize();
            }

            if (_loginServer == null)
            {
                _loginServer = new DdonLoginServer(_setting.LoginServerSetting, _database, _assetRepository);
            }

            if (_webServer == null)
            {
                _webServer = new DdonWebServer(_setting.WebServerSetting, _database);
            }

            if (_gameServer == null)
            {
                _gameServer = new DdonGameServer(_setting.GameServerSetting, _database, _assetRepository);
            }

            if (_rpcWebServer == null)
            {
                _rpcWebServer = new RpcWebServer(_webServer, _gameServer);
                _rpcWebServer.Init();
            }

            if (parameter.Arguments.Contains("start"))
            {
                _webServer.Start();
                _gameServer.Start();
                _loginServer.Start();

                if (isService)
                {
                    AutoResetEvent waitHandle = new AutoResetEvent(false);
                    bool sigintReceived = false;
                    Console.CancelKeyPress += (_, ea) =>
                    {
                        ea.Cancel = true;
                        Logger.Info("Received SIGINT (Ctrl+C)");
                        sigintReceived = true;
                        waitHandle.Set();
                    };
                    AppDomain.CurrentDomain.ProcessExit += (_, _) =>
                    {
                        if (sigintReceived)
                        {
                            return;
                        }

                        Logger.Info("Received SIGTERM");
                        waitHandle.Set();
                    };

                    waitHandle.WaitOne();
                    return CommandResultType.Exit;
                }

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
