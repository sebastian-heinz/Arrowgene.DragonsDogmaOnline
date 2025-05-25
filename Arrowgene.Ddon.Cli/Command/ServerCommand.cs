using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.LoginServer;
using Arrowgene.Ddon.Rpc.Web;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.WebServer;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ServerCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ServerCommand));

        private readonly Setting _setting;
        private DdonLoginServer _loginServer;
        private DdonGameServer _gameServer;
        private ServerScriptManager _serverScriptManager;
        private DdonWebServer _webServer;
        private RpcWebServer _rpcWebServer;
        private IDatabase _database;
        private AssetRepository _assetRepository;

        public string Key => "server";

        public string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";


        public ServerCommand(Setting setting)
        {
            _setting = setting;
        }


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

            Logger.Info("*** INFO ***");
            Logger.Info($"GameServerVersion: {Util.GetVersion("GameServer")}");
            Logger.Info($"Startup: {DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)}");
            Logger.Info($"OS: {Environment.OSVersion}");
            Logger.Info($"x64 OS: {Environment.Is64BitOperatingSystem}");
            Logger.Info($"Processors: {Environment.ProcessorCount}");
            Logger.Info($".NET Version: {Environment.Version}");
            Logger.Info($"x64 Process: {Environment.Is64BitProcess}");
            Logger.Info($"CurrentDirectory: {Environment.CurrentDirectory}");
            Logger.Info($"ApplicationDirectory: {Util.ExecutingDirectory()}");
            Logger.Info($"RelativeApplicationDirectory: {Util.RelativeExecutingDirectory()}");
            Logger.Info($"***{Environment.NewLine}");

            Logger.Info("*** DEBUG ***");
            Logger.Info("Locations:");
            foreach (string location in Util.DebugGetLocations())
            {
                Logger.Info($"- {location}");
            }

            Logger.Info("Versions:");
            foreach (Dictionary<string, string> version in Util.DebugGetVersions())
            {
                Logger.Info($"+ name: {version["name"]}");
                foreach (string key in version.Keys)
                {
                    if (key == "name")
                    {
                        continue;
                    }

                    Logger.Info($"- {key}: {version[key]}");
                }
            }

            Logger.Info($"***{Environment.NewLine}");

            bool isService = parameter.Switches.Contains("--service");

            if (_database == null)
            {
                _setting.DatabaseSetting ??= new DatabaseSetting();
                _database = DdonDatabaseBuilder.Build(_setting.DatabaseSetting);
            }

            uint currentDatabaseVersion = _database.GetMeta().DatabaseVersion;
            if (currentDatabaseVersion != DdonDatabaseBuilder.Version)
            {
                Logger.Error($"Database version is {currentDatabaseVersion}. Please update the database to version {DdonDatabaseBuilder.Version}.");
                Logger.Error("You can do this by running the server with the \"dbmigration\" flag.");
                return CommandResultType.Exit;
            }

            if (_assetRepository == null)
            {
                _assetRepository = new AssetRepository(_setting.AssetPath);
                _assetRepository.Initialize();
            }

            if (_serverScriptManager == null)
            {
                _serverScriptManager = new ServerScriptManager(_setting.AssetPath);
                _serverScriptManager.Initialize();
            }

            if (_loginServer == null)
            {
                _loginServer = new DdonLoginServer(_setting.LoginServerSetting, _serverScriptManager.GameServerSettingsModule.GameSettings, _database, _assetRepository);
            }

            if (_webServer == null)
            {
                _webServer = new DdonWebServer(_setting.WebServerSetting, _database);
            }

            if (_gameServer == null)
            {
                _gameServer = new DdonGameServer(_setting.GameServerSetting, _serverScriptManager.GameServerSettingsModule.GameSettings, _database, _assetRepository);
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
                _database.Stop();
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

            if (_database is not null)
            {
                _database.Stop();
            }
        }
    }
}
