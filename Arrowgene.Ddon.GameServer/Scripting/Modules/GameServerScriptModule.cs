using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server.Scripting;
using Arrowgene.Ddon.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public abstract class GameServerScriptModule : ScriptModule
    {
        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(AccountStateType).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(AssetRepository).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(DdonGameServer).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(DynamicAttribute).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(LibDdon).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(LibUtils).Assembly.Location))
                .AddImports("System", "System.Linq")
                .AddImports("System.Collections")
                .AddImports("System.Collections.Generic")
                .AddImports("System.Collections.ObjectModel")
                .AddImports("System.Runtime.CompilerServices")
                .AddImports("Arrowgene.Ddon.Database")
                .AddImports("Arrowgene.Ddon.Database.Model")
                .AddImports("Arrowgene.Ddon.GameServer")
                .AddImports("Arrowgene.Ddon.GameServer.Characters")
                .AddImports("Arrowgene.Ddon.GameServer.Chat")
                .AddImports("Arrowgene.Ddon.GameServer.Context")
                .AddImports("Arrowgene.Ddon.GameServer.Enemies")
                .AddImports("Arrowgene.Ddon.GameServer.Party")
                .AddImports("Arrowgene.Ddon.GameServer.Quests")
                .AddImports("Arrowgene.Ddon.GameServer.Quests.Extensions")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting.Interfaces")
                .AddImports("Arrowgene.Ddon.Server.Scripting")
                .AddImports("Arrowgene.Ddon.Shared")
                .AddImports("Arrowgene.Ddon.Shared.Entity.PacketStructure")
                .AddImports("Arrowgene.Ddon.Shared.Entity.Structure")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest");
        }
    }
}
