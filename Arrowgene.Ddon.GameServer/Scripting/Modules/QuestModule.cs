using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.IO;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class QuestModule : GameServerScriptModule
    {
        public override string ModuleRoot => "quests";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        // Contains a list of scripts which are interfaces or abstract classes
        // and doesn't attempt to compile them as a quest object
        private HashSet<string> IgnoredScripts = [
            "EmblemTrial.csx"
        ];

        public QuestModule()
        {
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            if (IgnoredScripts.Contains(Path.GetFileName(path)))
            {
                return true;
            }

            IQuest quest = (IQuest)result.ReturnValue;
            if (quest == null)
            {
                return false;
            }

            // Initialize any state required
            quest.Initialize(path);

            // TODO: Load quest through a different Mechanism
            LibDdon.LoadQuest(quest);

            return true;
        }
    }
}
