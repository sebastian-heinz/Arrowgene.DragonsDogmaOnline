using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class AreaPointCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "areapoint";
        public override string HelpText => "usage: `/areapoint [areaid] [amount]` - Gain area points.";

        private DdonGameServer _server;

        public AreaPointCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            QuestAreaId areaId = 0;
            if (command.Length == 0)
            {
                responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
                return;
            }

            if (command.Length >= 1)
            {
                if (uint.TryParse(command[0], out uint parsedId) && parsedId >= 1 && parsedId <= 21)
                {
                    areaId = (QuestAreaId)parsedId;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid areaId \"{command[0]}\". It must be a number (1~21)."));
                    return;
                }
            }

            uint amount = 0;
            if (command.Length >= 2)
            {
                if (uint.TryParse(command[1], out uint parsedAmount))
                {
                    amount = parsedAmount;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid amount \"{command[1]}\". It must be a number."));
                    return;
                }
            }

            _server.AreaRankManager.AddAreaPoint(client, areaId, amount).Send();
        }
    }
}
