using System.Collections.Generic;

namespace Arrowgene.Ddon.Cli
{
    public class CommandParameter
    {
        public CommandParameter(string key)
        {
            Key = key;
            Arguments = new List<string>();
            Switches = new List<string>();
            ArgumentMap = new Dictionary<string, string>();
            SwitchMap = new Dictionary<string, string>();
        }

        public string Key { get; }
        public List<string> Arguments { get; }
        public List<string> Switches { get; }
        public Dictionary<string, string> SwitchMap { get; }
        public Dictionary<string, string> ArgumentMap { get; }

        public void Clear()
        {
            Arguments.Clear();
            Switches.Clear();
            ArgumentMap.Clear();
            SwitchMap.Clear();
        }
    }
}
