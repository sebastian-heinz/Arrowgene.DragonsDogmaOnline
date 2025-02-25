using Arrowgene.Ddon.Server.Scripting.utils;
using System;
using System.Linq;
using System.Reflection;

namespace Arrowgene.Ddon.Server.Settings
{
    public abstract class IGameSettings
    {
        private string ScriptName { get; set; }
        private ScriptableSettings SettingsData { get; set; }
        public IGameSettings(ScriptableSettings settingsData, string scriptName)
        {
            SettingsData = settingsData;
            ScriptName = scriptName;

            // Create a new entry in the settings storage for this script
            SettingsData.CreateScriptStorage(scriptName);
        }

        protected T GetSetting<T>(string key)
        {
            return SettingsData.Get<T>(ScriptName, key);
        }

        protected T TryGetSetting<T>(string key, T defaultValue)
        {
            return SettingsData.TryGet<T>(ScriptName, key, defaultValue);
        }

        protected void SetSetting<T>(string key, T value)
        {
            SettingsData.Set(ScriptName, key, value);
        }

        public T Get<T>(string scriptName, string key)
        {
            return SettingsData.Get<T>(scriptName, key);
        }

        public T TryGet<T>(string scriptName, string key, T defaultValue)
        {
            return SettingsData.TryGet<T>(scriptName, key, defaultValue);
        }

        public void InitializeDefaults()
        {
            PropertyInfo[] properties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToArray();

            // Iterate over all properties and assign the default
            foreach (PropertyInfo prop in properties)
            {
                try
                {
                    prop.SetValue(this, prop.GetValue(this));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to set property for {ScriptName}::{prop.Name}");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
