using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetCraftRecipeHandler : GameStructurePacketHandler<C2SCraftRecipeGetCraftRecipeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetCraftRecipeHandler));

        public CraftRecipeGetCraftRecipeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftRecipeGetCraftRecipeReq> packet)
        {
            List<CDataMDataCraftRecipe> allRecipesInCategory = Server.AssetRepository.CraftingRecipesAsset
                .Where(recipes => recipes.Category == packet.Structure.Category)
                .Select(recipes => recipes.RecipeList)
                .SingleOrDefault(new List<CDataMDataCraftRecipe>());

            foreach (CDataMDataCraftRecipe cDataMDataCraftRecipe in allRecipesInCategory)
            {
                // TODO: All furniture recipes by default should be hidden - check which furniture recipes have been unlocked for player via achievements
                if (cDataMDataCraftRecipe.RecipeID == 270001)
                {
                    cDataMDataCraftRecipe.IsHide = false;
                }
            }

            client.Send(new S2CCraftRecipeGetCraftRecipeRes
            {
                Category = packet.Structure.Category,
                RecipeList = allRecipesInCategory
                    .SkipWhile(recipe => recipe.IsHide)
                    .Skip((int)packet.Structure.Offset)
                    .Take(packet.Structure.Num)
                    .ToList(),
                IsEnd = (packet.Structure.Offset + packet.Structure.Num) >= allRecipesInCategory.Count
            });
        }
    }
}
