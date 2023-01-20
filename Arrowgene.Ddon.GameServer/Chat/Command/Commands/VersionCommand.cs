using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    /// <summary>
    /// Provides information about the running server version
    /// </summary>
    public class VersionCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "version";
        public override string HelpText => "usage: `/version` - Provides information about the running server version";

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            ChatResponse response = new ChatResponse();
            response.Message = Util.GetVersion("GameServer");
            responses.Add(response);
            responses.Add(ChatResponse.ServerMessage(client, "Command Executed"));
        }
    }
}
