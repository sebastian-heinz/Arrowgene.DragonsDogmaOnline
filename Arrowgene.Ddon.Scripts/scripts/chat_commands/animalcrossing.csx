using System;
using System.Collections.Generic;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "animalcrossing";
    public override string HelpText => "usage: `/animalcrossing` - Unlock all furniture.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        var furnitureIds = server.AssetRepository.ClientItemInfos.Values.Where(x => x.Category == 6).Select(x => x.ItemId);
        var recipeIds = server.AssetRepository.ClientItemInfos.Values.Where(x => x.Category == 7).Select(x => x.ItemId);

        server.Database.ExecuteInTransaction(connection =>
        {
            foreach (var id in furnitureIds) 
            {
                server.Database.InsertUnlockedItem(client.Character.CharacterId, UnlockableItemCategory.FurnitureItem, id, connection);
            }
            foreach (var id in recipeIds)
            {
                server.Database.InsertUnlockedItem(client.Character.CharacterId, UnlockableItemCategory.CraftingRecipe, id, connection);
            }
        });

        client.Character.UnlockableItems.UnionWith(furnitureIds.Select(x => (UnlockableItemCategory.FurnitureItem, x)).ToHashSet());
        responses.Add(ChatResponse.ServerMessage(client, "All furniture unlocked."));
    }
}

return new ChatCommand();
