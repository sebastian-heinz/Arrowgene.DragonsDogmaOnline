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
        public CraftStartCraftHandler(DdonGameServer server) : base(server)
        {
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
            string RefineMaterialUID = packet.Structure.RefineMaterialUID;
            var RefineMaterialItem = Server.Database.SelectStorageItemByUId(RefineMaterialUID);
            byte RandomQuality = 0;
            int D100 =  Random.Shared.Next(100);
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
            
            // Check if a refinematerial is set
            if (!string.IsNullOrEmpty(RefineMaterialUID))
            {
                if (RefineMaterialItem.ItemId == 8036 || RefineMaterialItem.ItemId == 8068 ) // Checking if its one of the better rocks because they augment the odds of +3.
                {
                    D100 += 40;
                }
                else if (RefineMaterialItem.ItemId == 8052 || RefineMaterialItem.ItemId == 8084)
                {
                    D100 += 60;
                };
                List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, RefineMaterialUID, 1);
                updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                D100 += 10;
            }

            var thresholds = new (int Threshold, int Quality)[]
            {
                (100, 3),
                (90, 2),
                (70, 1),
                (0, 0)  // This should always be the last one to catch all remaining cases
            };

            RandomQuality = (byte)thresholds.First(t => D100 >= t.Threshold).Quality;

            // TODO: Refactor to generate the actual item here,
            // TODO: Quality is an innate principle, we need to check if the newly generated item belongs to certain subcats,found in ItemSubCategory,
            // Weapons & Armor can have quality, but subweapons (shield/red), lantern and jewelry cannot, even though the client does support it, its not meant to happen.
            // So we will have to filter them out.

            //TODO: There are 3 tiers of quality up items, we need to handle those appropriately. Looks like they don't get sent in the request in a special way,
            // So we'll need todo a direct ItemID comparison to know which one we're getting.
            


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