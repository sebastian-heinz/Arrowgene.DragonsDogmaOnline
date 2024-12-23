using Arrowgene.Ddon.GameServer.Scripting;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using static Arrowgene.Ddon.Server.ServerScriptManager;

namespace Arrowgene.Ddon.Shared.Scripting
{
    public abstract class ScriptManager<T>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptManager<T>));

        protected Dictionary<string, ScriptModule> ScriptModules { get; private set; }
        public string ScriptsRoot { get; private set; }
        public T GlobalVariables { get; protected set; }

        public ScriptManager(string assetsPath)
        {
            ScriptModules = new Dictionary<string, ScriptModule>();
            ScriptsRoot = $"{assetsPath}\\scripts";
        }

        public abstract void Initialize();

        protected void Initialize(T globalVariables)
        {
            GlobalVariables = globalVariables;

            CompileScripts();
            SetupFileWatchers();
        }

        protected void CompileScript(ScriptModule module, string path)
        {
            try
            {
                Logger.Info(path);

                var script = CSharpScript.Create(
                    code: Util.ReadAllText(path),
                    options: module.Options(),
                    globalsType: typeof(T)
                );

                var result = script.RunAsync(GlobalVariables).Result;
                if (!module.EvaluateResult(path, result))
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

        protected void CompileScripts()
        {
            foreach (var module in ScriptModules.Values)
            {
                var path = $"{ScriptsRoot}\\{module.ModuleRoot}";

                Logger.Info($"Compiling scripts for module '{module.ModuleRoot}'");
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    if (Path.GetExtension(file) != ".csx")
                    {
                        continue;
                    }

                    module.Scripts.Add(file);
                    CompileScript(module, file);
                }
            }
        }

        private void SetupFileWatchers()
        {
            foreach (var module in ScriptModules.Values)
            {
                if (!module.EnableHotLoad)
                {
                    continue;
                }

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
                if (module.EnableHotLoad)
                {
                    module.Watcher.EnableRaisingEvents = true;
                }
            }
        }
        protected void OnChanged(object sender, FileSystemEventArgs e)
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
