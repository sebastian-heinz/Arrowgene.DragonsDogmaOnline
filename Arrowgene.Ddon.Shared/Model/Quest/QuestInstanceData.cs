using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestInstanceVars
    {
        private Dictionary<string, object> Data;

        public QuestInstanceVars()
        {
            Data = new Dictionary<string, object>();
        }

        public T GetData<T>(string name)
        {
            if (!Data.ContainsKey(name))
            {
                throw new Exception($"QuestInstanceData doesn't contain a field for {name}");
            }

            return (T) Data[name];
        }

        public void SetData<T>(string key, T value)
        {
            Data[key] = value;
        }
    }
}
