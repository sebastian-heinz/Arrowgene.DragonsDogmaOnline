using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetCraftRecipeDesignateHandler : GameRequestPacketHandler<C2SCraftRecipeGetCraftRecipeDesignateReq, S2CCraftRecipeGetCraftRecipeDesignateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetCraftRecipeDesignateHandler));

        public CraftRecipeGetCraftRecipeDesignateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftRecipeGetCraftRecipeDesignateRes Handle(GameClient client, C2SCraftRecipeGetCraftRecipeDesignateReq request)
        {
            S2CCraftRecipeGetCraftRecipeDesignateRes res = new()
            {
                Category = request.Category,
                ItemList = request.ItemList,
            };

            var allRecipes = Server.AssetRepository.CraftingRecipesAsset
                    .Where(x => x.Category == request.Category)
                    .SelectMany(x => x.RecipeList)
                    .Where(x => x.UnlockID > 0);

            foreach ( var item in request.ItemList)
            {
                res.RecipeList.AddRange(allRecipes.Where(x => x.UnlockID == item.Value).Select(x => x.AsCData()));
            }

            return res;
        }
    }
}
