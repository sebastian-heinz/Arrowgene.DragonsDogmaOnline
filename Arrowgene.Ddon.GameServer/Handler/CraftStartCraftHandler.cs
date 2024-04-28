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

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartCraftHandler : GameStructurePacketHandler<C2SCraftStartCraftReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartCraftHandler));
        
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
                int consumedItems = 0;

                // Take first from item bag
                CDataItemUpdateResult? craftMaterialBagUpdateResult = _itemManager.ConsumeItemByUIdFromItemBag(Server, client.Character, craftMaterial.ItemUId, craftMaterial.ItemNum);
                if(craftMaterialBagUpdateResult != null)
                {
                    updateCharacterItemNtc.UpdateItemList.Add(craftMaterialBagUpdateResult);
                    consumedItems += craftMaterialBagUpdateResult.UpdateItemNum;
                }
                
                // If there weren't enough items in the item bag, take from the storage box
                if (craftMaterial.ItemNum + consumedItems  >  0)
                {
                    CDataItemUpdateResult? craftMaterialStorageUpdateResult = _itemManager.ConsumeItemByUId(Server, client.Character, StorageType.StorageBoxNormal, craftMaterial.ItemUId, craftMaterial.ItemNum);
                    if(craftMaterialStorageUpdateResult != null)
                    {
                        updateCharacterItemNtc.UpdateItemList.Add(craftMaterialStorageUpdateResult);
                        consumedItems += craftMaterialStorageUpdateResult.UpdateItemNum;
                    }
                }

                // TODO: GG storage box? Other storages?

                if (craftMaterial.ItemNum + consumedItems != 0)
                {
                    // TODO: Rollback transaction
                    Logger.Error("Consumed "+consumedItems+" items of UID "+craftMaterial.ItemUId+" but the crafting recipe required "+craftMaterial.ItemNum);
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
            CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.Gold).Single();
            wallet.Value = Math.Max(0, wallet.Value - finalCraftCost);
            Database.UpdateWalletPoint(client.Character.CharacterId, wallet);
            updateCharacterItemNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
            {
                Type = WalletType.Gold,
                AddPoint = (int)-finalCraftCost,
                Value = wallet.Value
            });

            // Add crafted items
            CDataItemUpdateResult? itemUpdateResult = _itemManager.AddItem(Server, client.Character, false, recipe.ItemID, packet.Structure.CreateCount * recipe.Num);
            updateCharacterItemNtc.UpdateItemList.Add(itemUpdateResult);

            client.Send(updateCharacterItemNtc);
            client.Send(new S2CCraftStartCraftRes());
        }
    }
}