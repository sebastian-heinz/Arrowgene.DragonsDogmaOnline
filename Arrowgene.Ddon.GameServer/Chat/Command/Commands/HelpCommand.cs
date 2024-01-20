using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class HelpCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "help";
        public override string HelpText => "usage: `/help` - Shows this text";

        private Dictionary<string, ChatCommand> ChatCommands;

        public HelpCommand(Dictionary<string, ChatCommand> chatCommands)
        {
            ChatCommands = chatCommands;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            this.ChatCommands.Values
                .Select(cmd => new ChatResponse() {Message = cmd.HelpText})
                .ToList()
                .ForEach(response => responses.Add(response));
        }
    }
}
