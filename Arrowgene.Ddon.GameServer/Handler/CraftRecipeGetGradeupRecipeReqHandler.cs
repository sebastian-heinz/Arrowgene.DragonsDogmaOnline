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

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetGradeupRecipeReqHandler : GameStructurePacketHandler<C2SGetCraftGradeupRecipeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetGradeupRecipeReqHandler));

        private readonly DdonGameServer _server;


                private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };

        private readonly ItemManager _itemManager;
        public CraftRecipeGetGradeupRecipeReqHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SGetCraftGradeupRecipeReq> packet)
        {
            CDataMDataCraftRecipe recipe = Server.AssetRepository.CraftingRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => recipe.RecipeID == packet.Structure.Category)
                .Single();
                
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

            Logger.Debug("Packet stuff is going downnnnn");

            
        }
    }
}
