using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Server.Settings;

namespace Arrowgene.Ddon.Server
{
    public class GameSettings
    {
        private ScriptableSettings SettingsData { get; set; }
        public GameServerSettings GameServerSettings { get; private set; }
        public SeasonalEventSettings SeasonalEventsSettings { get; private set; }
        public PointModifierSettings PointModifiersSettings { get; private set; }
        public ChatCommandSettings ChatCommandsSettings { get; private set; }
        public EmblemSettings EmblemSettings { get; private set; }

        public GameSettings(ScriptableSettings settingsData)
        {
            SettingsData = settingsData;
            GameServerSettings = new GameServerSettings(SettingsData);
            SeasonalEventsSettings = new SeasonalEventSettings(SettingsData);
            PointModifiersSettings = new PointModifierSettings(SettingsData);
            ChatCommandsSettings = new ChatCommandSettings(SettingsData);
            EmblemSettings = new EmblemSettings(SettingsData);
        }

        public T Get<T>(string scriptName, string variableName)
        {
            return SettingsData.Get<T>(scriptName, variableName);
        }
    }
}
