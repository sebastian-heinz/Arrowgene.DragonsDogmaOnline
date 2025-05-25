#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Craft;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartCraftHandler : GameRequestPacketHandler<C2SCraftStartCraftReq, S2CCraftStartCraftRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartCraftHandler));

        public static readonly HashSet<ItemSubCategory> NoQualitySubCategories = new()
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

        public override S2CCraftStartCraftRes Handle(GameClient client, C2SCraftStartCraftReq request)
        {
            CraftingRecipe recipe;
            try
            {
                recipe = Server.AssetRepository.CraftingRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Single(recipe => recipe.RecipeID == request.RecipeID);
            }
            catch (InvalidOperationException)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_RECIPE_NOT_FOUND, $"Duplicate recipe ID {request.RecipeID}");
            }
            
            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, recipe.ItemID);

            ushort AddStatusID = request.AdditionalStatusId;
            CDataAddStatusParam AddStat = new CDataAddStatusParam()
            {
                IsAddStat1 = false,
                IsAddStat2 = false,
                AdditionalStatus1 = 0,
                AdditionalStatus2 = 0,
            };
            List<CDataAddStatusParam> AddStatList = new List<CDataAddStatusParam>();

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.StartCraft
            };

            Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);

            List<CraftPawn> craftPawns = new()
            {
                new CraftPawn(leadPawn, CraftPosition.Leader)
            };
            craftPawns.AddRange(request.CraftSupportPawnIDList.Select(p => new CraftPawn(Server.CraftManager.FindPawn(client, p.PawnId), CraftPosition.Assistant)));
            craftPawns.AddRange(request.CraftMasterLegendPawnIDList.Select(p => new CraftPawn(Server.AssetRepository.PawnCraftMasterLegendAsset.Single(m => m.PawnId == p.PawnId))));

            PacketQueue packets = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                // Remove crafting materials
                foreach (var craftMaterial in request.CraftMaterialList)
                {
                    try
                    {
                        List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes,
                            craftMaterial.ItemUId, craftMaterial.ItemNum, connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                    }
                    catch (NotEnoughItemsException)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has Occurred.");
                    }
                }

                double calculatedOdds = CraftManager.CalculateEquipmentQualityIncreaseRate(craftPawns);
                uint plusValue = 0;
                bool isGreatSuccessEquipmentQuality = false;
                bool canPlusValue = !NoQualitySubCategories.Contains(itemInfo.SubCategory);
                if (canPlusValue && !string.IsNullOrEmpty(request.RefineMaterialUID))
                {
                    Item refineMaterialItem = Server.Database.SelectStorageItemByUId(request.RefineMaterialUID, connection);
                    CraftCalculationResult craftCalculationResult = CraftManager.CalculateEquipmentQuality(refineMaterialItem, (uint)calculatedOdds);
                    plusValue = craftCalculationResult.CalculatedValue;
                    isGreatSuccessEquipmentQuality = craftCalculationResult.IsGreatSuccess;

                    try
                    {
                        List<CDataItemUpdateResult> updateResults =
                            Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, request.RefineMaterialUID, 1, connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                    }
                    catch (NotEnoughItemsException)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has Occurred.");
                    }
                }

                uint consumableAdditionalQuantity = 0;
                bool isGreatSuccessConsumableQuantity = false;
                if (itemInfo.StorageType == StorageType.ItemBagConsumable)
                {
                    CraftCalculationResult craftCalculationResult = CraftManager.CalculateConsumableQuantity(craftPawns, (uint)calculatedOdds);
                    consumableAdditionalQuantity = request.CreateCount * craftCalculationResult.CalculatedValue;
                    isGreatSuccessConsumableQuantity = craftCalculationResult.IsGreatSuccess;
                }

                CraftProgress craftProgress = new CraftProgress
                {
                    CraftCharacterId = client.Character.CharacterId,
                    CraftLeadPawnId = request.CraftMainPawnID,
                    CraftSupportPawnId1 = request.CraftSupportPawnIDList.ElementAtOrDefault(0)?.PawnId ?? 0,
                    CraftSupportPawnId2 = request.CraftSupportPawnIDList.ElementAtOrDefault(1)?.PawnId ?? 0,
                    CraftSupportPawnId3 = request.CraftSupportPawnIDList.ElementAtOrDefault(2)?.PawnId ?? 0,
                    RecipeId = request.RecipeID,
                    NpcActionId = NpcActionType.NpcActionSmithy,
                    ItemId = recipe.ItemID,
                    AdditionalStatusId = request.AdditionalStatusId,
                    // TODO: implement mechanism to deduct time periodically
                    RemainTime = Server.CraftManager.CalculateRecipeProductionSpeed(recipe.Time, itemInfo, craftPawns),
                    CreateCount = recipe.Num * request.CreateCount,
                    PlusValue = plusValue,
                    GreatSuccess = isGreatSuccessEquipmentQuality || isGreatSuccessConsumableQuantity,
                    AdditionalQuantity = consumableAdditionalQuantity
                };

                // TODO: check if course bonus provides exp bonus for crafting & calculate bonus EXP
                // TODO: Decide whether bonus exp should be calculated when craft is started vs. received
                bool expBonus = false;
                if (CraftManager.CanPawnExpUp(leadPawn))
                {
                    craftProgress.Exp = recipe.Exp * request.CreateCount;
                    craftProgress.ExpBonus = expBonus;
                    if (expBonus)
                    {
                        craftProgress.BonusExp = craftProgress.Exp * 2;
                    }
                }
                else
                {
                    craftProgress.Exp = 0;
                    craftProgress.ExpBonus = false;
                    craftProgress.BonusExp = 0;
                }

                Server.Database.InsertPawnCraftProgress(craftProgress, connection);
                foreach (CraftPawn pawn in craftPawns)
                {
                    if (pawn.Pawn != null)
                    {
                        pawn.Pawn.PawnState = PawnState.Craft;
                        if (pawn.Pawn.PawnType == PawnType.Main)
                        {
                            Server.Database.UpdatePawnBaseInfo(pawn.Pawn, connection);
                        }
                    }
                }

                // Subtract craft price
                uint cost = Server.CraftManager.CalculateRecipeCost(recipe.Cost, itemInfo, craftPawns) * request.CreateCount;
                CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, cost, connection)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INSUFFICIENT_GOLD, $"Insufficient gold. {cost} > {Server.WalletManager.GetWalletAmount(client.Character, WalletType.Gold)}"); updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

                if (request.CraftMasterLegendPawnIDList.Count > 0)
                {
                    uint totalGPcost = (uint)request.CraftMasterLegendPawnIDList.Sum(p => Server.AssetRepository.PawnCraftMasterLegendAsset.Single(m => m.PawnId == p.PawnId).RentalCost);
                    CDataUpdateWalletPoint updateGP = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.GoldenGemstones, totalGPcost, connection)
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
                    updateCharacterItemNtc.UpdateWalletList.Add(updateGP);
                }

                if (leadPawn.PawnId == client.Character.PartnerPawnId)
                {
                    packets.AddRange(Server.PartnerPawnManager.UpdateLikabilityIncreaseAction(client, PartnerPawnAffectionAction.Craft, connection));
                }
            });

            packets.Send();

            client.Send(updateCharacterItemNtc);
            return new S2CCraftStartCraftRes();
        }
    }
}
