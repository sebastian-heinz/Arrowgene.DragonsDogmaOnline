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
        private static readonly HashSet<ItemSubCategory> BannedSubCategories = new HashSet<ItemSubCategory>
        {
            ItemSubCategory.WeaponShield,
            ItemSubCategory.WeaponRod,
            ItemSubCategory.EquipJewelry,
            ItemSubCategory.EquipLantern,
            ItemSubCategory.JewelryCommon,
            ItemSubCategory.JewelryRing,
            ItemSubCategory.JewelryBracelet,
            ItemSubCategory.JewelryPierce,
            ItemSubCategory.EmblemStone,
            ItemSubCategory.EquipOverwear,
            ItemSubCategory.EquipClothingBody,
            ItemSubCategory.EquipClothingLeg,
        };
        public CraftStartCraftHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartCraftReq> packet)
        {       
            bool CanPlusValue = false;
            bool IsGreatSuccess = false;
            CDataMDataCraftRecipe recipe = Server.AssetRepository.CraftingRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => recipe.RecipeID == packet.Structure.RecipeID)
                .Single();

            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, recipe.ItemID);
            if (itemInfo.SubCategory.HasValue && BannedSubCategories.Contains(itemInfo.SubCategory.Value))
            {
                CanPlusValue = false;
            }
            else
            {
                CanPlusValue = true;
            };


            // TODO: Run in transaction

            // TODO: Validate the info in the packet is consistent with the recipe

            // TODO: Instead of giving the player the item immediately
            // save the crafting to DB, and notify the player when the craft
            // time passes.
            string RefineMaterialUID = packet.Structure.RefineMaterialUID;
            var RefineMaterialItem = Server.Database.SelectStorageItemByUId(RefineMaterialUID);
            byte RandomQuality = 0;

            ushort AddStatusID = packet.Structure.Unk0;
            CDataAddStatusParam AddStat = new CDataAddStatusParam()
            {
                IsAddStat1 = false,
                IsAddStat2 = false,
                AdditionalStatus1 = 0,
                AdditionalStatus2 = 0,
            };
            List<CDataAddStatusParam> AddStatList = new List<CDataAddStatusParam>();

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = ItemNoticeType.CraftCreate;

            // Remove crafting materials
            foreach (var craftMaterial in packet.Structure.CraftMaterialList)
            {
                try
                {
                    List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, craftMaterial.ItemUId, craftMaterial.ItemNum);
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

            if (CanPlusValue == true)
            {
                byte GreatSuccessValue = 1;
                byte GreatSuccessOdds = 10;

                if (!string.IsNullOrEmpty(RefineMaterialUID)) // Check if a refinematerial is set
                {
                    RandomQuality = 1;
                    if (RefineMaterialItem.ItemId == 8036 || RefineMaterialItem.ItemId == 8068 ) // Checking if its one of the better rocks because they augment the odds of +3.
                    {
                        RandomQuality = 2; // Quality rocks gurantee a minimum, standard is 1, Quality and WhiteDragon are 2.
                        GreatSuccessValue = 3; // Quality Rocks determine the highest you can roll, standard is +2, Quality and WhiteDragon are +3. (Max requires greatsuccess)
                    }
                    else if (RefineMaterialItem.ItemId == 8052 || RefineMaterialItem.ItemId == 8084)
                    {
                        RandomQuality = 2;
                        GreatSuccessValue = 3;
                        GreatSuccessOdds = 5; // WhiteDragon Rocks have better odds of GreatSuccess.
                    };

                    List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, RefineMaterialUID, 1);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }

                IsGreatSuccess = Random.Shared.Next(GreatSuccessOdds) == 0;

                if (IsGreatSuccess)
                {
                    RandomQuality = GreatSuccessValue;
                }
            };



            // TODO: Refactor to generate the actual item here,
            // TODO: Calculate final craft price with the discounts from the craft pawns
            uint finalCraftCost = recipe.Cost * packet.Structure.CreateCount;

            // Temporary solution for craft price when setting a second pawn of rank 1
            // Temporary solution for Quality result, need to take pawn high quality into account instead.
            if(packet.Structure.CraftSupportPawnIDList.Count > 0)
            {
                finalCraftCost = (uint)(finalCraftCost*0.95);
            }

            // Substract craft price
            CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, finalCraftCost);
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

            // Add crafted items
            List<CDataItemUpdateResult> itemUpdateResult = Server.ItemManager.AddItem(Server, client.Character, false, recipe.ItemID, packet.Structure.CreateCount * recipe.Num, RandomQuality);
            updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResult);                                                       

            client.Send(updateCharacterItemNtc);
            client.Send(new S2CCraftStartCraftRes());
        }
    }
}