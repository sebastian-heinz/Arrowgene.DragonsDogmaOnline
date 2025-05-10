using Arrowgene.Ddon.GameServer.Scripting;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Scripting
{
    public abstract class ScriptManager<T>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptManager<T>));

        protected Dictionary<string, ScriptModule> ScriptModules { get; private set; }
        public string ScriptsRoot { get; private set; }
        public string LibsRoot { get; private set; } = string.Empty;
        public T GlobalVariables { get; protected set; }
        public List<string> PathsToIgnore { get; protected set; }

        public ScriptManager(string assetsPath, string libsPath)
        {
            ScriptModules = new Dictionary<string, ScriptModule>();
            ScriptsRoot = Path.Combine(assetsPath, "scripts");
            PathsToIgnore = new List<string>();

            if (libsPath != "")
            {
                LibsRoot = Path.Combine(ScriptsRoot, libsPath);
            }
        }

        protected void AddModule(ScriptModule module)
        {
            ScriptModules[module.ModuleRoot] = module;
        }

        public abstract void Initialize();

        protected void Initialize(T globalVariables)
        {
            GlobalVariables = globalVariables;

            CompileScripts();
            SetupFileWatchers();
        }

        /// <summary>
        /// To debug scripts which include other scripts, we need emit
        /// the compiled script as a dll so the debugger can find the 
        /// symbols and source files.
        /// </summary>
        /// <param name="script">The compiled script object</param>
        /// <param name="path">Path to the main script being executed</param>
        private void EmitScriptsAsDllForDebug(Script script, string path)
        {
            // Put the debug assemblies in <asset_path>/net9.0/Files
            var assembliesPath = Path.Combine(ScriptsRoot, "../../script_assemblies");
            if (!Directory.Exists(assembliesPath))
            {
                Directory.CreateDirectory(assembliesPath);
            }

            var compilation = script.GetCompilation();

            var outputPath = Path.Combine(assembliesPath, $"{Path.GetFileNameWithoutExtension(path)}.dll");
            var emitOptions = new EmitOptions()
                .WithDebugInformationFormat(DebugInformationFormat.Pdb)
                .WithPdbFilePath(outputPath);

            using (var stream = new FileStream(outputPath, FileMode.Create))
            {
                var compilationResult = compilation.Emit(stream, options: emitOptions);
                if (!compilationResult.Success)
                {
                    foreach (var diagnostic in compilationResult.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                }
            }
        }

        protected async void CompileScript(ScriptModule module, string path)
        {
            try
            {
                Logger.Info(path);

                var code = Util.ReadAllText(path);

                var options = module.Options()
#if DEBUG
                    .WithFilePath(path)
                    .WithEmitDebugInformation(true)
#endif
                    .WithFileEncoding(Encoding.UTF8);

                if (LibsRoot != "")
                {
                    options = options.WithSourceResolver(new SourceFileResolver(
                        searchPaths: new [] { Path.GetDirectoryName(path), LibsRoot},
                        baseDirectory: Path.GetDirectoryName(path)
                    ));
                }
                
                var script = CSharpScript.Create(
                    code: code,
                    options: options,
                    globalsType: typeof(T)
                );

#if DEBUG
                EmitScriptsAsDllForDebug(script, path);
#endif

                var result = await script.RunAsync(globals: GlobalVariables);
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

        private string GetCustomPath(string path, string moduleRoot)
        {
            return path.Replace($"{ScriptsRoot}{Path.DirectorySeparatorChar}{moduleRoot}",
                                $"{ScriptsRoot}{Path.DirectorySeparatorChar}custom{Path.DirectorySeparatorChar}{moduleRoot}");
        }

        /// <summary>
        /// Returns if pathA contain pathB.
        /// </summary>
        /// <param name="pathA">The path to find pathB in.</param>
        /// <param name="pathB">The path to find pathA in.</param>
        /// <returns>Returns true if pathB is in pathA, otherwise false.</returns>
        private bool PathContains(string pathA, string pathB)
        {
            string normalizedA = Path.GetFullPath(pathA).TrimEnd(Path.DirectorySeparatorChar);
            string normalizedB = Path.GetFullPath(pathB).TrimEnd(Path.DirectorySeparatorChar);
            return normalizedA.Contains(normalizedB);
        }

        private bool ShouldIgnoreFile(string path)
        {
            foreach (var pathToIgnore in PathsToIgnore)
            {
                if (PathContains(path, pathToIgnore))
                {
                    return true;
                }
            }
            return false;
        }

        protected void CompileScripts()
        {
            Dictionary<string, ScriptModule>.ValueCollection scriptModulesValues = ScriptModules.Values;
            Parallel.ForEach(scriptModulesValues, module =>
            {
                var path = Path.Combine(ScriptsRoot, module.ModuleRoot);
                if (!module.IsEnabled)
                {
                    Logger.Info($"The module '{module.ModuleRoot}' is disabled. Skipping.");
                    return;
                }

                module.Initialize();

                Logger.Info($"Compiling scripts for module '{module.ModuleRoot}'");
                foreach (var filePath in Directory.GetFiles(path, "*.csx", SearchOption.AllDirectories))
                {
                    var fileToCompile = filePath;
                    if (ShouldIgnoreFile(fileToCompile))
                    {
                        continue;
                    }

                    var overlayFilePath = GetCustomPath(filePath, module.ModuleRoot);
                    if (File.Exists(overlayFilePath))
                    {
                        fileToCompile = overlayFilePath;
                    }

                    module.Scripts.Add(fileToCompile);
                    CompileScript(module, fileToCompile);
                }
            });
        }

        private void SetupFileWatchers()
        {
            foreach (var module in ScriptModules.Values)
            {
                if (!module.IsEnabled || !module.EnableHotLoad)
                {
                    continue;
                }

                var modulePaths = new List<string>()
                {
                    $"{ScriptsRoot}{Path.DirectorySeparatorChar}{module.ModuleRoot}",
                    $"{ScriptsRoot}{Path.DirectorySeparatorChar}custom{Path.DirectorySeparatorChar}{module.ModuleRoot}",
                };

                foreach (var path in modulePaths)
                {
                    if (!Directory.Exists(path))
                    {
                        continue;
                    }

                    var watcher = new FileSystemWatcher(path);
                    watcher.Filter = module.Filter;
                    watcher.NotifyFilter = (NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime);
                    watcher.IncludeSubdirectories = module.ScanSubdirectories;

                    watcher.Changed += OnChanged;
                    watcher.Created += OnCreate;
                    watcher.Error += OnError;

                    module.Watchers.Add(watcher);
                }
            }

            // Enable all the watchers
            foreach (var module in ScriptModules.Values)
            {
                if (module.EnableHotLoad)
                {
                    foreach (var watcher in module.Watchers)
                    {
                        watcher.EnableRaisingEvents = true;
                    }
                }
            }
        }

        protected ScriptModule GetModuleFromFilePath(string path)
        {
            return ScriptModules.Values.FirstOrDefault(m => m.Scripts.Contains(path));
        }

        protected virtual void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            if (ShouldIgnoreFile(e.FullPath))
            {
                return;
            }

            var module = GetModuleFromFilePath(e.FullPath);
            if (module == null)
            {
                // No module associated with this script file
                return;
            }

            Logger.Info($"Reloading {e.FullPath}");
            try
            {
                foreach (var watcher in module.Watchers)
                {
                    watcher.EnableRaisingEvents = false;
                }
                CompileScript(module, e.FullPath);
            }
            finally
            {
                foreach (var watcher in module.Watchers)
                {
                    watcher.EnableRaisingEvents = true;
                }
            }
        }

        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            if (ShouldIgnoreFile(e.FullPath))
            {
                return;
            }

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
                foreach (var watcher in module.Watchers)
                {
                    watcher.EnableRaisingEvents = false;
                }
                CompileScript(module, e.FullPath);
            }
            finally
            {
                foreach (var watcher in module.Watchers)
                {
                    watcher.EnableRaisingEvents = true;
                }
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
