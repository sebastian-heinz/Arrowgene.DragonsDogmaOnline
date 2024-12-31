using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.Text;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class ChatCommandModule : ScriptModule
    {
        public override string ModuleRoot => "chat_commands";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public Dictionary<string, IChatCommand> Commands { get; private set; } = new Dictionary<string, IChatCommand>();

        public ChatCommandModule()
        {
        }

        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(DdonGameServer).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(AssetRepository).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(AccountStateType).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic", "System.Linq")
                .AddImports("Arrowgene.Ddon.Shared")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.GameServer")
                .AddImports("Arrowgene.Ddon.GameServer.Chat")
                .AddImports("Arrowgene.Ddon.GameServer.Characters")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting.Interfaces")
                .AddImports("Arrowgene.Ddon.Shared.Entity.PacketStructure")
                .AddImports("Arrowgene.Ddon.Shared.Entity.Structure")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest")
                .AddImports("Arrowgene.Ddon.Database")
                .AddImports("Arrowgene.Ddon.Database.Model");
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
