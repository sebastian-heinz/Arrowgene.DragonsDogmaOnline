using Arrowgene.Ddon.GameServer.Scripting;
using Arrowgene.Ddon.Server.Scripting.interfaces;
using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.IO;

namespace Arrowgene.Ddon.Server.Scripting.modules
{
    public class GameServerSettingsModule : ScriptModule
    {
        public override string ModuleRoot => "settings";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        /// <summary>
        /// Settings data is organized as ScriptName.FieldName
        /// </summary>
        private ScriptableSettings SettingsData { get; set; }
        public GameLogicSetting GameLogicSetting { get; private set; }

        public GameServerSettingsModule()
        {
            SettingsData = new ScriptableSettings();
            GameLogicSetting = new GameLogicSetting(SettingsData);
        }

        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(GameLogicSetting).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(WalletType).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(LibUtils).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic")
                .AddImports("Arrowgene.Ddon.Server.Scripting")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest");
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            var scriptName = Path.GetFileNameWithoutExtension(path);

            foreach (var variable in result.Variables)
            {
                SettingsData.Set(scriptName, variable.Name, variable.Value);
            }

            return true;
        }
    }
}
