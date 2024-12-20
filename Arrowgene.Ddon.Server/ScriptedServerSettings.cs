using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Arrowgene.Ddon.Server
{
    public class ScriptedServerSettings
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedServerSettings));

        public class Globals
        {
            public GameLogicSetting GameLogicSetting { get; set; }
        }

        private string ScriptsRoot { get; set; }
        private Dictionary<string, Script> CompiledScripts;
        private GameLogicSetting GameLogicSetting;
        public FileSystemWatcher Watcher { get; private set; }

        public Script GameLogicSettings
        {
            get
            {
                lock (CompiledScripts)
                {
                    return CompiledScripts["GameLogicSettings"];
                }
            }
            set
            {
                lock (CompiledScripts)
                {
                    CompiledScripts["GameLogicSettings"] = value;
                }
            }
        }

        public ScriptedServerSettings(GameLogicSetting gameLogicSetting, string AssetsPath)
        {
            ScriptsRoot = $"{AssetsPath}\\scripts";

            GameLogicSetting = gameLogicSetting;

            CompiledScripts = new Dictionary<string, Script>();

            var ScriptsDirectory = new DirectoryInfo(ScriptsRoot);
            if (!ScriptsDirectory.Exists)
            {
                return;
            }

            Watcher = SetupFileWatcher();
        }

        public void LoadSettings()
        {
            string settingsPath = $"{ScriptsRoot}\\GameLogicSettings.csx";

            var options = ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(GameLogicSetting).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(WalletType).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest");

            Globals globals = new Globals()
            {
                GameLogicSetting = GameLogicSetting
            };

            Logger.Info($"Loading Scriptable game settings from {ScriptsRoot}");
            Logger.Info($"{settingsPath}");

            var code = Util.ReadAllText(settingsPath);

            // Load The Game Settings
            GameLogicSettings = CSharpScript.Create(
                code: code,
                options: options,
                globalsType: typeof(Globals)
            );

            if (GameLogicSettings != null)
            {
                // Execute the script file to populate the settings
                GameLogicSettings.RunAsync(globals);
            }
        }

        private FileSystemWatcher SetupFileWatcher()
        {
            var watcher = new FileSystemWatcher(ScriptsRoot);
            watcher.Filter = "GameLogicSettings.csx";

            watcher.NotifyFilter = (NotifyFilters.LastWrite);

            watcher.Changed += OnChanged;
            watcher.Error   += OnError;
            watcher.EnableRaisingEvents = true;

            return watcher;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            Logger.Info($"Reloading {e.FullPath}");

            try
            {
                Watcher.EnableRaisingEvents = false;

                LoadSettings();
            }
            finally
            {
                Watcher.EnableRaisingEvents = true;
            }
        }

        private void OnError(object sender, ErrorEventArgs e) => 
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Logger.Error($"{ex.Message}");
                Logger.Error($"Stacktrace:");
                PrintException(ex.InnerException);
            }
        }
    }
}
