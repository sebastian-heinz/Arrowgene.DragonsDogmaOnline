#nullable enable
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GetCraftGradeupRecipeHandler : GameStructurePacketHandler<S2CGetCraftGradeupRecipeRes>
    {
            private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartCraftHandler));
            private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };

        private readonly ItemManager _itemManager;

        public GetCraftGradeupRecipeHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<S2CGetCraftGradeupRecipeRes> packet)
        {
            try
            {
                // Extract packet data
                byte category = packet.Structure.Category;
                List<CDataMDataCraftGradeupRecipe> recipeList = packet.Structure.RecipeList;
                List<CDataCommonU32> unknownItemList = packet.Structure.UnknownItemList;
                bool isEnd = packet.Structure.IsEnd;
                Logger.Debug($"Received crafting gradeup recipe response. Category: {category}, Recipes: {recipeList.Count}, IsEnd: {isEnd}");
            }
            catch (Exception caughtException)
            {
                // Log error
                Logger.Exception(caughtException);

                // Handle error as needed
            }
        }
    }
}
