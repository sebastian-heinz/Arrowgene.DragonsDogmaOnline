using System.Collections.Generic;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "givepowerfulitems";
    public override string HelpText               => "usage: `/givepowerfulitems` - Get a set of free stuff.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        var ntc = new S2CItemUpdateCharacterItemNtc()
        {
            UpdateType = ItemNoticeType.StampBonus
        };

        server.Database.ExecuteInTransaction(connection =>
        {
            foreach (var item in Items)
            {
                ntc.UpdateItemList.AddRange(server.ItemManager.AddItem(server, client.Character, StorageType.ItemPost, item, 1, connectionIn: connection));
            }
        });

        client.Send(ntc);
    }

    private static List<uint> Items = new List<uint>()
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
}

return new ChatCommand();
