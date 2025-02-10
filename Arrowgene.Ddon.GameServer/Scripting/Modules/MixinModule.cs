using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class MixinModule : GameServerScriptModule
    {
        public override string ModuleRoot => "mixins";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        private Dictionary<string, object> Mixins { get; set; }

        public MixinModule()
        {
            Mixins = new Dictionary<string, object>();
        }

        public T Get<T>(string scriptName)
        {
            if (!Mixins.ContainsKey(scriptName))
            {
                throw new Exception($"A mixin with the name '{scriptName}' doesn't exist");
            }

            return (T) Mixins[scriptName];
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            // Mixins are c# Func<> with arbitary return and params based on the functionality
            string mixinName = Path.GetFileNameWithoutExtension(path);
            Mixins[mixinName] = result.ReturnValue;

            return true;
        }
    }
}
