using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Server.Scripting.utils
{
    public class ScriptableSettings
    {
        /// <summary>
        /// Settings data is organized as ScriptName.FieldName
        /// </summary>
        private Dictionary<string, Dictionary<string, object>> Data { get; set; }

        public ScriptableSettings()
        {
            Data = new Dictionary<string, Dictionary<string, object>>();
        }

        public void CreateScriptStorage(string scriptName)
        {
            if (Data.ContainsKey(scriptName))
            {
                throw new Exception($"The script '{scriptName}' already exists, unable to create a new settings entry");
            }

            Data[scriptName] = new Dictionary<string, object>();
        }

        public Dictionary<string, object> GetScriptData(string scriptName)
        {
            if (!Data.ContainsKey(scriptName))
            {
                throw new Exception($"The script '{scriptName}' doesn't exist");
            }

            return Data[scriptName];
        }

        public T Get<T>(string scriptName, string variableName)
        {
            if (!Data.ContainsKey(scriptName))
            {
                throw new Exception($"The script '{scriptName}' doesn't exist");
            }

            if (!Data[scriptName].ContainsKey(variableName))
            {
                throw new Exception($"The setting '{scriptName}.{variableName}' doesn't exist");
            }
            return (T)Data[scriptName][variableName];
        }

        public T TryGet<T>(string scriptName, string variableName, T defaultValue)
        {
            if (!Data.ContainsKey(scriptName))
            {
                throw new Exception($"The script '{scriptName}' doesn't exist");
            }

            return Data[scriptName].TryGetValue(variableName, out object result) ? (T) result : defaultValue;
        }

        public void Set<T>(string scriptName, string variableName, T value)
        {
            if (!Data.ContainsKey(scriptName))
            {
                Data[scriptName] = new Dictionary<string, object>();
            }

            Data[scriptName][variableName] = value;
        }
    }
}
