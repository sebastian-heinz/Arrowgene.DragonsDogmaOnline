using Arrowgene.Ddon.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class MixinModule : ScriptModule
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

        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(DdonGameServer).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(AssetRepository).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(DynamicAttribute).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic")
                .AddImports("System.Runtime.CompilerServices")
                .AddImports("Arrowgene.Ddon.Shared")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.GameServer")
                .AddImports("Arrowgene.Ddon.GameServer.Characters")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting.Interfaces")
                .AddImports("Arrowgene.Ddon.Shared.Entity.PacketStructure")
                .AddImports("Arrowgene.Ddon.Shared.Entity.Structure")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest");
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
