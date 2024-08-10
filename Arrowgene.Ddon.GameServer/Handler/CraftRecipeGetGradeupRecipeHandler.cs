using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetGradeupRecipeHandler : GameRequestPacketHandler<C2SCraftRecipeGetCraftGradeupRecipeReq, S2CCraftRecipeGetCraftGradeupRecipeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetGradeupRecipeHandler));

        public CraftRecipeGetGradeupRecipeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftRecipeGetCraftGradeupRecipeRes Handle(GameClient client, C2SCraftRecipeGetCraftGradeupRecipeReq request)
        {
            List<CDataMDataCraftGradeupRecipe> allRecipesInCategory = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .Where(allRecipesInCategory => allRecipesInCategory.Category == request.Category)
                .Select(allRecipesInCategory => allRecipesInCategory.RecipeList)
                .SingleOrDefault( new List<CDataMDataCraftGradeupRecipe>());

            List<CDataCommonU32> ItemList = request.ItemList;

            var response = new S2CCraftRecipeGetCraftGradeupRecipeRes()
            {
                Category = request.Category,
                RecipeList = allRecipesInCategory
                    .Skip((int)request.Offset)
                    .Take((int)request.Num)
                    .ToList(),
                UnknownItemList = ItemList,
                IsEnd = (request.Offset + request.Num) >= allRecipesInCategory.Count
            };

            return response;
        }
    }
}
