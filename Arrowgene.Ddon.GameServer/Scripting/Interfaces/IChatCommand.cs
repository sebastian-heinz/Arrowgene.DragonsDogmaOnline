using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Chat;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IChatCommand
    {
        public virtual AccountStateType AccountState { get; }
        public abstract string CommandName { get; }
        public abstract string HelpText { get; }
        public abstract void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses);
    }
}
