using Arrowgene.Ddon.Database.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    internal class GivePowerfulItemsCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "givepowerfulitems";
        public override string HelpText => "usage: `/givepowerfulitems` - Get a set of free stuff.";

        private DdonGameServer _server;
        private GiveItemCommand _giveItem;

        public GivePowerfulItemsCommand(DdonGameServer server)
        {
            _server = server;
            _giveItem = new GiveItemCommand(server);
        }

        static List<uint> Items = new List<uint>()
        {
            25604,
            25606,
            25607,
            25609,
            25610,
            25611,
            25612,
            25613,
            25614,
            25615,
            25616,
            25605,
            25608,
            25621,
            25622
        };

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            foreach (var item in Items)
            {
                _giveItem.Execute(new[] { $"{item}" }, client, message, responses);
            }
        }
    }
}
