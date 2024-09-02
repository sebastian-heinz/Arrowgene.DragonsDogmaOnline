using Arrowgene.Ddon.Database.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class SkipTutorialCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "skiptutorial";
        public override string HelpText => "usage: `/skiptutorial` - Skip early annoying stuff.";

        private DdonGameServer _server;
        private FinishQuestCommand _finishQuest;
        private GivePawnCommand _givePawn;
        private ReleaseCommand _release;

        public SkipTutorialCommand(DdonGameServer server)
        {
            _server = server;
            _finishQuest = new FinishQuestCommand(server);
            _givePawn = new GivePawnCommand(server);
            _release = new ReleaseCommand(server);
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            _finishQuest.Execute(new[] { "1" }, client, message, responses);
            _finishQuest.Execute(new[] { "2" }, client, message, responses);
            _finishQuest.Execute(new[] { "3" }, client, message, responses);
            _finishQuest.Execute(new[] { "4" }, client, message, responses);
            _finishQuest.Execute(new[] { "26" }, client, message, responses);
            _givePawn.Execute(new string[] { }, client, message, responses);
            _release.Execute(new string[] { }, client, message, responses);
        }
    }
}
