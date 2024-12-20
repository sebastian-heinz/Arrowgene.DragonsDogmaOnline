using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class ScriptManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptManager));
        public class GlobalVariables
        {
            public GlobalVariables(DdonGameServer server)
            {
                Server = server;
            }

            public DdonGameServer Server { get; }
        };

        public NpcExtendedFacilityModule NpcExtendedFacilityModule { get; private set; } = new NpcExtendedFacilityModule();

        private Dictionary<string, ScriptModule> ScriptModules;

        public ScriptManager(DdonGameServer server)
        {
            Server = server;
            ScriptsRoot = $"{server.AssetRepository.AssetsPath}\\scripts";

            ScriptModules = new Dictionary<string, ScriptModule>()
            {
                {NpcExtendedFacilityModule.ModuleRoot, NpcExtendedFacilityModule}
            };

            Globals = new GlobalVariables(Server);
        }

        private DdonGameServer Server { get; }
        private string ScriptsRoot { get; }
        private GlobalVariables Globals { get; }

        public void Initialize()
        {
            CompileScripts();
            SetupFileWatchers();
        }

        private void CompileScript(ScriptModule module, string path)
        {
            try
            {
                Logger.Info(path);

                var script = CSharpScript.Create(
                    code: Util.ReadAllText(path),
                    options: module.Options(),
                    globalsType: typeof(GlobalVariables)
                );

                var result = script.RunAsync(Globals).Result;
                if (!module.EvaluateResult(result))
                {
                    Logger.Error($"Failed to evaluate the result of executing '{path}'");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to compile and execute '{path}'. Skipping.");
                Logger.Error(ex.ToString());
            }
        }

        private void CompileScripts()
        {
            foreach (var module in ScriptModules.Values)
            {
                var path = $"{ScriptsRoot}\\{module.ModuleRoot}";

                Logger.Info($"Compiling scripts for module '{module.ModuleRoot}'");
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    module.Scripts.Add(file);
                    CompileScript(module, file);
                }
            }
        }

        private void SetupFileWatchers()
        {
            foreach (var module in ScriptModules.Values)
            {
                var watcher = new FileSystemWatcher($"{ScriptsRoot}\\{module.ModuleRoot}");
                watcher.Filter = module.Filter;
                watcher.NotifyFilter = (NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime);
                watcher.IncludeSubdirectories = module.ScanSubdirectories;

                watcher.Changed += OnChanged;
                watcher.Created += OnCreate;
                watcher.Error += OnError;

                module.Watcher = watcher;
            }

            // Enable all the watchers
            foreach (var module in ScriptModules.Values)
            {
                module.Watcher.EnableRaisingEvents = true;
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            Logger.Info($"Reloading {e.FullPath}");

            ScriptModule module = null;
            foreach (var m in ScriptModules.Values)
            {
                if (m.Scripts.Contains(e.FullPath))
                {
                    module = m;
                    break;
                }
            }

            if (module == null)
            {
                // No module associated with this script file
                return;
            }

            try
            {
                module.Watcher.EnableRaisingEvents = false;
                CompileScript(module, e.FullPath);
            }
            finally
            {
                module.Watcher.EnableRaisingEvents = true;
            }
        }

        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            var module = ScriptUtils.FindModule(e.FullPath, ScriptModules);
            if (module == null)
            {
                Logger.Error($"Unable to find module associated with '{e.FullPath}'. You may need to reload the server or fix error.");
                return;
            }

            // Add this file to be tracked for the module
            module.Scripts.Add(e.FullPath);

            Logger.Info($"Compiling script for module '{module.ModuleRoot}'");
            try
            {
                module.Watcher.EnableRaisingEvents = false;
                CompileScript(module, e.FullPath);
            }
            finally
            {
                module.Watcher.EnableRaisingEvents = true;
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
