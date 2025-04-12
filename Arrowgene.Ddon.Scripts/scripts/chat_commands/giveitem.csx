using System.Collections;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "giveitem";
    public override string HelpText               => "usage: `/giveitem <itemid> [amount?] [force?]` - Obtain items.";

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
                responses.Add(ChatResponse.CommandError(client, $"Invalid itemId \"{command[0]}\". It must be a number."));
                return;
            }
        }

        uint amount = 1;
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

        bool force = false;
        if (command.Length >= 3)
        {
            if (bool.TryParse(command[2], out bool parsedForce))
            {
                force = parsedForce;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid force \"{command[2]}\". It must be a bool."));
                return;
            }
        }

        if (!server.AssetRepository.ClientItemInfos.ContainsKey(itemId))
        {
            responses.Add(ChatResponse.CommandError(client, $"Invalid itemId \"{command[0]}\". This item does not exist."));
            return;
        }

        var ntc = new S2CItemUpdateCharacterItemNtc()
        {
            UpdateType = ItemNoticeType.Gather
        };

        var (queue, isSpecial) = server.ItemManager.HandleSpecialItem(client, ntc, (ItemId)itemId, amount);
        if (!isSpecial)
        {
            ntc.UpdateItemList.AddRange(server.ItemManager.AddItem(server, client.Character, true, itemId, amount));
        }

        queue.Send();
        client.Send(ntc);
    }
}

return new ChatCommand();
