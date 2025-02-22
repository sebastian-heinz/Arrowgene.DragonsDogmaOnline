public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "arearank";
    public override string HelpText => "usage: `/arearank [areaid] [rank]` - Set area rank.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
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

        AreaRank rank = client.Character.AreaRanks.GetValueOrDefault(areaId) 
            ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
        rank.Rank = amount;
        server.Database.UpdateAreaRank(client.Character.CharacterId, rank);
        responses.Add(ChatResponse.ServerMessage(client, $"Setting {areaId} to rank {amount}..."));
    }
}

return new ChatCommand();
