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
                // .Where(recipes => recipes.Category == packet.Structure.Category) // I think the game wants to do this? but it only returns Recipe 1 if we do this.
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => packet.Structure.ItemList.Any(itemId => itemId.Value == recipe.ItemID))
                .ToList();
                
            var res = new S2CCraftRecipeGetCraftGradeupRecipeRes()
            {
                Category = packet.Structure.Category,
                RecipeList = allRecipesInCategory
                    .Skip((int)packet.Structure.Offset)
                    .Take((int)packet.Structure.Num)
                    .ToList(),
                IsEnd = (packet.Structure.Offset+packet.Structure.Num) >= allRecipesInCategory.Count,
            };
            client.Send(res);
            }
                
        }
    }

