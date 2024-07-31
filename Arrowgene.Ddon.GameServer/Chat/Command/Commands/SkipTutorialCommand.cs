using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class SkipTutorialCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "skiptutorial";
        public override string HelpText => "usage: `/skiptutorial` - Skip early annoying stuff.";

        private DdonGameServer _server;

        public SkipTutorialCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            _server.ChatManager.Handle(client, new ChatMessage(LobbyChatMsgType.Say, 0, 0, 0, "/finishquest 1"));
            _server.ChatManager.Handle(client, new ChatMessage(LobbyChatMsgType.Say, 0, 0, 0, "/finishquest 2"));
            _server.ChatManager.Handle(client, new ChatMessage(LobbyChatMsgType.Say, 0, 0, 0, "/finishquest 3"));
            _server.ChatManager.Handle(client, new ChatMessage(LobbyChatMsgType.Say, 0, 0, 0, "/finishquest 4"));
            _server.ChatManager.Handle(client, new ChatMessage(LobbyChatMsgType.Say, 0, 0, 0, "/finishquest 26"));
            _server.ChatManager.Handle(client, new ChatMessage(LobbyChatMsgType.Say, 0, 0, 0, "/givepawn"));
            _server.ChatManager.Handle(client, new ChatMessage(LobbyChatMsgType.Say, 0, 0, 0, "/release"));
        }
    }
}
