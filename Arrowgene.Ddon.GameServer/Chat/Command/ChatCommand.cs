using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Command
{
    public abstract class ChatCommand
    {
        public abstract void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses);

        public abstract AccountStateType AccountState { get; }
        public abstract string Key { get; }
        public string KeyToLowerInvariant => Key.ToLowerInvariant();

        public virtual string HelpText
        {
            get { return null; }
        }
    }
}
