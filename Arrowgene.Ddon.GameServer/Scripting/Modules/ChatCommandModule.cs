using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class ChatCommandModule : GameServerScriptModule
    {
        public override string ModuleRoot => "chat_commands";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public Dictionary<string, IChatCommand> Commands { get; private set; } = new Dictionary<string, IChatCommand>();

        public ChatCommandModule()
        {
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            IChatCommand command = (IChatCommand)result.ReturnValue;
            if (command != null)
            {
                Commands[command.CommandName.ToLowerInvariant()] = command; 
            }

            return true;
        }
    }
}
