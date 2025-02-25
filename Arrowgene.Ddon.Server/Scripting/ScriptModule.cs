using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.IO;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public abstract class ScriptModule
    {
        public abstract string ModuleRoot { get; }
        public abstract string Filter { get; }
        public abstract bool ScanSubdirectories { get; }
        public List<FileSystemWatcher> Watchers { get; set; }

        /// <summary>
        /// Determines if this module is able to be hot-loadable or not.
        /// If a module is not hot-loadable, it will only be evaluated once
        /// when the server first loads.
        /// </summary>
        public abstract bool EnableHotLoad { get; }

        /// <summary>
        /// Determines if this module is enabled or not. Intended for development;
        /// should be used such that an in-development module can be checked in but
        /// disabled until the feature is ready.
        /// </summary>
        public virtual bool IsEnabled { get; } = true;

        public HashSet<string> Scripts { get; set; }

        public ScriptModule()
        {
            Scripts = new HashSet<string>();
            Watchers = new List<FileSystemWatcher>();
        }

        /// <summary>
        /// Code that a module can execute before EvaluateResult is called for each script.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Options passed into the compilation unit.
        /// </summary>
        /// <returns></returns>
        public abstract ScriptOptions Options();

        /// <summary>
        /// Evaluates the result returned by the script.
        /// </summary>
        /// <param name="path">Path to the script that was executed</param>
        /// <param name="result">The result object of the script that executed</param>
        /// <returns></returns>
        public abstract bool EvaluateResult(string path, ScriptState<object> result);
    }
}
