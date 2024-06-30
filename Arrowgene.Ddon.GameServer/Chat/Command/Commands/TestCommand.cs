using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    /// <summary>
    /// Example Command Handler
    /// </summary>
    public class TestCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "test";
        public override string HelpText => "usage: `/test [command]` - A test chat command, replies with the provided command";

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            if (command.Length <= 0)
            {
                // check expected length before accessing
                responses.Add(ChatResponse.CommandError(client, "no arguments provided"));
                return;
            }

            ChatResponse response = new ChatResponse();
            response.Message = string.Join(' ', command);
            responses.Add(response);

            responses.Add(ChatResponse.ServerMessage(client, "Command Executed"));
        }
    }
}
