using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server.Network;
using System.ComponentModel;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetGradeupRecipeHandler : GameStructurePacketHandler<C2SGetCraftGradeupRecipeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetGradeupRecipeHandler));

        public CraftRecipeGetGradeupRecipeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SGetCraftGradeupRecipeReq> packet)
        {
            List<CDataMDataCraftGradeupRecipe> allRecipesInCategory = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .Where(recipes => recipes.Category == packet.Structure.Category)
                .Select(recipes => recipes.RecipeList)
                .SingleOrDefault(new List<CDataMDataCraftGradeupRecipe>());

            var res = new S2CGetCraftGradeupRecipeRes()
            {
                Category = packet.Structure.Category,
                RecipeList = allRecipesInCategory
                    .Skip((int) packet.Structure.Offset)
                    .Take(packet.Structure.Num)
                    .ToList(),
                IsEnd = (packet.Structure.Offset+packet.Structure.Num) >= allRecipesInCategory.Count
            };
            client.Send(res);
            }
                
        }
    }

