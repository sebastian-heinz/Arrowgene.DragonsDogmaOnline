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
        public FileSystemWatcher Watcher { get; set; }

        public HashSet<string> Scripts { get; set; }

        public ScriptModule()
        {
            Scripts = new HashSet<string>();
        }

        /// <summary>
        /// Options passed into the compilation unit.
        /// </summary>
        /// <returns></returns>
        public abstract ScriptOptions Options();

        /// <summary>
        /// Evaluates the result returned by the script.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public abstract bool EvaluateResult(ScriptState<object> result);
    }
}
