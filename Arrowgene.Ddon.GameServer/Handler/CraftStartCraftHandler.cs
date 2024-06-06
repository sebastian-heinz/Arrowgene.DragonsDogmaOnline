#nullable enable
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
    public class CraftStartCraftHandler : GameStructurePacketHandler<C2SCraftStartCraftReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartCraftHandler));

        private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };

        private readonly ItemManager _itemManager;

        public CraftStartCraftHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartCraftReq> packet)
        {
            CDataMDataCraftRecipe recipe = Server.AssetRepository.CraftingRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => recipe.RecipeID == packet.Structure.RecipeID)
                .Single();

            // TODO: Run in transaction

            // TODO: Validate the info in the packet is consistent with the recipe

            // TODO: Instead of giving the player the item immediately
            // save the crafting to DB, and notify the player when the craft
            // time passes.

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

            // Remove crafting materials
            foreach (var craftMaterial in packet.Structure.CraftMaterialList)
            {
                try
                {
                    List<CDataItemUpdateResult> updateResults = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, craftMaterial.ItemUId, craftMaterial.ItemNum);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
                catch (NotEnoughItemsException e)
                {
                    Logger.Exception(e);
                    client.Send(new S2CCraftStartCraftRes()
                    {
                        Result = 1
                    });
                    return;
                }
            }

            // TODO: Refining material and all that stuff

            // TODO: Calculate final craft price with the discounts from the craft pawns
            uint finalCraftCost = recipe.Cost * packet.Structure.CreateCount;

            // Temporary solution for craft price when setting a second pawn of rank 1
            // TODO: Remove
            if(packet.Structure.CraftSupportPawnIDList.Count > 0)
            {
                finalCraftCost = (uint)(finalCraftCost*0.95);
            }

            // Substract craft price
            CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, finalCraftCost);
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

            // Add crafted items
            List<CDataItemUpdateResult> itemUpdateResult = _itemManager.AddItem(Server, client.Character, false, recipe.ItemID, packet.Structure.CreateCount * recipe.Num);
            updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResult);

            client.Send(updateCharacterItemNtc);
            client.Send(new S2CCraftStartCraftRes());
        }
    }
}