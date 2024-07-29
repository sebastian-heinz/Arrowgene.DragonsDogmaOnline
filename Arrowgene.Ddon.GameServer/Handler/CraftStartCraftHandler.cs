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
        private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> 
        {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };
        private readonly Random _random;
        public CraftStartCraftHandler(DdonGameServer server) : base(server)
        {
            _random = new Random();
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
            string RefineMaterial = packet.Structure.RefineMaterialUID;
            byte RandomQuality = 0;
            int D100 =  _random.Next(100);
            ushort AddStatusID = packet.Structure.Unk0;
            CDataAddStatusData AddStat = new CDataAddStatusData()
            {
                IsAddStat1 = 0,
                IsAddStat2 = 0,
                AdditionalStatus1 = 0,
                AdditionalStatus2 = 0,
            };
            List<CDataAddStatusData> AddStatList = new List<CDataAddStatusData>();

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

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
            
            // Check if a refinematerial is set
            if (!string.IsNullOrEmpty(RefineMaterial))
            {
                // Remove Refinement material (and increase odds of better Stars)
                foreach (var craftMaterial in packet.Structure.CraftMaterialList)
                {
                    try
                    {
                        List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, RefineMaterial, 1);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                        
                        var thresholds = new (int Threshold, int Quality)[]
                        {
                            (95, 3),
                            (80, 2),
                            (70, 1),
                            (0, 0)  // This should always be the last one to catch all remaining cases
                        };

                        RandomQuality = (byte)thresholds.First(t => D100 >= t.Threshold).Quality;
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
            }
            if (AddStatusID > 0)
            {
                //bool success = Server.Database.InsertAddStatus(equipItemUID, charid, 1, 0, AddStatusID, 0);
                // TODO: Refactor this code to create the new Item here, and add the Quality & AdditionalStatus like that. I can't get the UID for the addstatus entries, for example.

                AddStat = new CDataAddStatusData()
                {
                    IsAddStat1 = 1,
                    IsAddStat2 = 0,
                    AdditionalStatus1 = AddStatusID,
                    AdditionalStatus2 = 0,
                };
                AddStatList = new List<CDataAddStatusData>()
                {
                    AddStat
                };
            };


            // TODO: Remove most logic since it should happen in the handler for when the craft time completes.
            


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