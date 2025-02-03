using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Microsoft.CodeAnalysis.Scripting;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class QuestModule : GameServerScriptModule
    {
        public override string ModuleRoot => "quests";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public QuestModule()
        {
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
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
