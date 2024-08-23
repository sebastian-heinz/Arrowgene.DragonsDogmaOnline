using Arrowgene.Ddon.GameServer.Scripting;
using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Server.Settings;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
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

        public GameSettings GameSettings { get; private set; }

        private List<IGameSettings> DefaultSettings { get; set; }

        public string TemplatesDirectory {get; private set;}

        public GameServerSettingsModule(string scriptsRoot)
        {
            TemplatesDirectory = Path.Combine(scriptsRoot, "settings/templates");

            SettingsData = new ScriptableSettings();
            GameSettings = new GameSettings(SettingsData);

            // Provide a list of all the settings objects
            // that defaults need to be assigned for
            DefaultSettings = new List<IGameSettings>()
            {
                GameSettings.GameServerSettings,
                GameSettings.ChatCommandsSettings,
                GameSettings.SeasonalEventsSettings,
                GameSettings.PointModifiersSettings,
                GameSettings.EmblemSettings,
            };
        }

        private void CreateTemplate(string outputRootPath, Type type)
        {
            string outputPath = Path.Combine(outputRootPath, $"{type.Name}.csx");
            SettingsTemplate.Generate(outputPath, type);
        }

        public override void Initialize()
        {
            if (!Directory.Exists(TemplatesDirectory))
            {
                Directory.CreateDirectory(TemplatesDirectory);
            }

            foreach (var defaultSetting in DefaultSettings)
            {
                defaultSetting.InitializeDefaults();
                CreateTemplate(TemplatesDirectory, defaultSetting.GetType());
            }
        }

        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(GameSettings).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(WalletType).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(LibUtils).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic")
                .AddImports("Arrowgene.Ddon.Server.Scripting")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest");
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (path.Contains(TemplatesDirectory))
            {
                return true;
            }

            var scriptName = Path.GetFileNameWithoutExtension(path);

            foreach (var variable in result.Variables)
            {
                SettingsData.Set(scriptName, variable.Name, variable.Value);
            }

            return true;
        }
    }
}
