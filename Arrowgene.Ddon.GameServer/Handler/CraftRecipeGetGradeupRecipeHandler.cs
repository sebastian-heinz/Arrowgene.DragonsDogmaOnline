using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetGradeupRecipeHandler : GameStructurePacketHandler<C2SCraftRecipeGetCraftGradeupRecipeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetGradeupRecipeHandler));

        public CraftRecipeGetGradeupRecipeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftRecipeGetCraftGradeupRecipeReq> packet)
        {
            List<CDataMDataCraftGradeupRecipe> allRecipesInCategory = Server.AssetRepository.CraftingGradeUpRecipesAsset
                //.Where(recipes => recipes.Category == packet.Structure.Category)
                // TODO: Figure out why trying to filter by Category ^ doesn't work, it doesn't populate anything below the highest Category Number,
                // and even that list doesn't fully populate correctly.
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => packet.Structure.ItemList.Any(itemId => itemId.Value == recipe.ItemID))
                .ToList();

            // To populate the UnknownItemList in Response with the current Itemlist, this seems to fix the infinite req/res spam soft lock.
            List<CDataCommonU32> testlist = packet.Structure.ItemList;
                
            var res = new S2CCraftRecipeGetCraftGradeupRecipeRes()
            {
                Category = packet.Structure.Category,
                RecipeList = allRecipesInCategory
                    .Skip((int)packet.Structure.Offset)
                    .Take((int)packet.Structure.Num)
                    .ToList(),
                UnknownItemList = testlist, // Unknown why but populating this list fixes the infinite req/res spam, additionally the client is tracking upgrade progress.
                IsEnd = (packet.Structure.Offset+packet.Structure.Num) >= allRecipesInCategory.Count
            };
            client.Send(res);
            }
                
        }
    }

