using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetCraftRecipeHandler : GameRequestPacketHandler<C2SCraftRecipeGetCraftRecipeReq, S2CCraftRecipeGetCraftRecipeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetCraftRecipeHandler));

        public CraftRecipeGetCraftRecipeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftRecipeGetCraftRecipeRes Handle(GameClient client, C2SCraftRecipeGetCraftRecipeReq request)
        {
            var allRecipesInCategory = new List<CDataMDataCraftRecipe>();
            if (request.Category == RecipeCategory.LimitBreak)
            {
                allRecipesInCategory = Server.CraftManager.DeterminePromotionRecipies();
            }
            else
            {
                allRecipesInCategory = Server.AssetRepository.CraftingRecipesAsset
                    .Where(recipes => recipes.Category == request.Category)
                    .Select(recipes => recipes.RecipeList)
                    .SingleOrDefault(new List<CDataMDataCraftRecipe>());
            }

            // TODO: All furniture & ensemble recipes available via achievements by default should be hidden in the JSON, here we must check which recipes the player has unlocked via achievements
            foreach (CDataMDataCraftRecipe cDataMDataCraftRecipe in allRecipesInCategory)
            {
                // Currently 270000 represents Mini Table, i.e. Achievement #530 Bounty Hunter
                if (cDataMDataCraftRecipe.RecipeID == 270000)
                {
                    cDataMDataCraftRecipe.IsHide = false;
                }
            }

            return new S2CCraftRecipeGetCraftRecipeRes
            {
                Category = request.Category,
                RecipeList = allRecipesInCategory
                    .SkipWhile(recipe => recipe.IsHide)
                    .Skip((int)request.Offset)
                    .Take(request.Num)
                    .ToList(),
                IsEnd = (request.Offset + request.Num) >= allRecipesInCategory.Count
            };
        }
    }
}
