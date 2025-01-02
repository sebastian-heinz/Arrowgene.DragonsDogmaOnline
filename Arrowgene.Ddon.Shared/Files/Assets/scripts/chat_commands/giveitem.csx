public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "giveitem";
    public override string HelpText               => "usage: `/giveitem <itemid> [amount?]` - Obtain items.";

    private const uint DefaultAmount = 1;
    private const bool DefaultToStorage = false;

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        uint itemId = 0;
        if (command.Length == 0)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
            return;
        }

        if (command.Length >= 1)
        {
            if (uint.TryParse(command[0], out uint parsedId))
            {
                itemId = parsedId;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid itemId \"{command[0]}\". It must be a number"));
                return;
            }
        }

        uint amount = DefaultAmount;
        if (command.Length >= 2)
        {
            if (uint.TryParse(command[1], out uint parsedAmount))
            {
                amount = parsedAmount;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid amount \"{command[1]}\". It must be a number"));
                return;
            }
        }

        if (!server.AssetRepository.ClientItemInfos.ContainsKey(itemId))
        {
            responses.Add(ChatResponse.CommandError(client, $"Invalid itemId \"{command[0]}\". This item does not exist."));
            return;
        }

        client.Send(new S2CItemUpdateCharacterItemNtc()
        {
            UpdateType = ItemNoticeType.StampBonus,
            UpdateItemList = server.ItemManager.AddItem(server, client.Character, StorageType.ItemPost, itemId, amount),
        });
    }
}

return new ChatCommand();