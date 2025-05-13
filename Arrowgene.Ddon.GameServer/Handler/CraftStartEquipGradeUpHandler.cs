#nullable enable
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Database.Model;
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
    public class CraftStartEquipGradeUpHandler : GameRequestPacketQueueHandler<C2SCraftStartEquipGradeUpReq, S2CCraftStartEquipGradeUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipGradeUpHandler));
        private readonly ItemManager _itemManager;
        private readonly CraftManager _craftManager;

        public CraftStartEquipGradeUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
            _craftManager = Server.CraftManager;
        }

        public override PacketQueue Handle(GameClient client, C2SCraftStartEquipGradeUpReq request)
        {
            PacketQueue queue = new();

            string equipItemUID = request.EquipItemUID;
            Character character = client.Character;
            var ramItem = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            Item equipItem = ramItem.Item2.Item2;
            uint charid = client.Character.CharacterId;
            uint craftpawnid = request.CraftMainPawnID;
            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItem.ItemId);

            // Fetch the crafting recipe data for the item
            CDataMDataCraftGradeupRecipe recipeData = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .First(recipe => recipe.ItemID == equipItem.ItemId);

            uint gearUpgradeID = recipeData.GradeupItemID;
            uint goldRequired = recipeData.Cost;
            UpgradableStatus upgradableStatus = recipeData.Upgradable;
            uint pawnExp = recipeData.Exp;
            bool canContinue = true;
            bool doUpgrade = false;
            uint currentTotalEquipPoint = equipItem.EquipPoints;

            CDataCurrentEquipInfo currentEquipInfo = new()
            {
                ItemUId = equipItemUID,
            };

            // More dummy data, looks like its DragonAugment related.
            CDataCraftStartEquipGradeUpUnk0Unk0 dragonAugmentData = new();
            CDataCraftStartEquipGradeUpUnk0 dummydata = new() // TODO: Figure this out
            {
                Unk0 = new List<CDataCraftStartEquipGradeUpUnk0Unk0> { dragonAugmentData },
                DragonAugment = false, // makes the DragonAugment slot popup appear if set to true.
            };

            S2CCraftStartEquipGradeUpRes res = new();
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new();

            Server.Database.ExecuteInTransaction(connection =>
            {
                // Removes crafting materials
                foreach (var craftMaterial in request.CraftMaterialList)
                {
                    try
                    {
                        var updateResults =
                            _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, craftMaterial.ItemUId, craftMaterial.ItemNum, connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                    }
                    catch (NotEnoughItemsException)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has Occurred.");
                    }
                }


                Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);

                List<CraftPawn> craftPawns = new()
                {
                    new CraftPawn(leadPawn, CraftPosition.Leader)
                };
                craftPawns.AddRange(request.CraftSupportPawnIDList.Select(p => new CraftPawn(Server.CraftManager.FindPawn(client, p.PawnId), CraftPosition.Assistant)));
                craftPawns.AddRange(request.CraftMasterLegendPawnIDList.Select(p => new CraftPawn(Server.AssetRepository.PawnCraftMasterLegendAsset.Single(m => m.PawnId == p.PawnId))));

                double calculatedOdds = CraftManager.CalculateEquipmentQualityIncreaseRate(craftPawns);
                CraftCalculationResult enhnacementResult = _craftManager.CalculateEquipmentEnhancement(craftPawns, (uint)calculatedOdds);
                bool isGreatSuccess = enhnacementResult.IsGreatSuccess;
                uint addEquipPoint = enhnacementResult.CalculatedValue;

                currentTotalEquipPoint += addEquipPoint;

                uint cost = Server.CraftManager.CalculateRecipeCost(recipeData.Cost, itemInfo, craftPawns);
                CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, cost, connection)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INSUFFICIENT_GOLD, $"Insufficient gold. {cost} > {Server.WalletManager.GetWalletAmount(client.Character, WalletType.Gold)}"); updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

                updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

                if (request.CraftMasterLegendPawnIDList.Count > 0)
                {
                    uint totalGPcost = (uint)request.CraftMasterLegendPawnIDList.Sum(p => Server.AssetRepository.PawnCraftMasterLegendAsset.Single(m => m.PawnId == p.PawnId).RentalCost);
                    CDataUpdateWalletPoint updateGP = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.GoldenGemstones, totalGPcost, connection)
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
                    updateCharacterItemNtc.UpdateWalletList.Add(updateGP);
                }

                byte currentStars = (byte)itemInfo.Quality;
                uint remainingPoints = currentTotalEquipPoint;
                List<CDataCommonU32> gradeupList = new List<CDataCommonU32>();

                uint[] thresholds = { 350, 700, 1000, 1500, 800 };

                // Determine the required points based on the current star level
                uint requiredPoints = currentStars >= 0 && currentStars < thresholds.Length 
                    ? thresholds[currentStars] 
                    : throw new InvalidOperationException("Invalid star level");

                if (recipeData.AllowMultiGrade)
                {
                    if (currentTotalEquipPoint >= requiredPoints)
                    {
                        doUpgrade = true;
                        List<CDataMDataCraftGradeupRecipe> itemIDsList = FindRecipeFamily(recipeData);
                        remainingPoints = currentTotalEquipPoint;
                        int thresholdsExceeded = 0;

                        for (int i = currentStars; i < thresholds.Length; i++)
                        {
                            if (remainingPoints >= thresholds[i])
                            {
                                remainingPoints -= thresholds[i];
                                thresholdsExceeded++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        gradeupList = itemIDsList.Take(thresholdsExceeded).Select(recipe => new CDataCommonU32(recipe.GradeupItemID)).ToList();
                        upgradableStatus = itemIDsList.Take(thresholdsExceeded).LastOrDefault().Upgradable;
                        gearUpgradeID = gradeupList.Count > 0 ? gradeupList.Last().Value : 0;
                    }
                }
                else
                {
                    if (currentTotalEquipPoint >= requiredPoints)
                    {
                        currentTotalEquipPoint -= requiredPoints;

                        if (currentStars < 4)
                        {
                            int nextThresholdIndex = currentStars + 1;
                            uint nextThreshold = thresholds[nextThresholdIndex];

                            // Cap remaining points or just update them based on the next threshold
                            remainingPoints = Math.Min(currentTotalEquipPoint, nextThreshold - 1);
                        }
                        else
                        {
                            remainingPoints = currentTotalEquipPoint;
                        }
                        // Prepare to grade up
                        gradeupList = new() { new CDataCommonU32(gearUpgradeID) };
                        doUpgrade = true;
                    }
                }
                if (upgradableStatus == UpgradableStatus.No)
                {
                    canContinue = false;
                }

                if (doUpgrade)
                {
                    equipItem.ItemId = gearUpgradeID;
                    if (canContinue)
                    {
                        currentTotalEquipPoint = remainingPoints;
                    }
                    else
                    {
                        currentTotalEquipPoint = 0;
                    }

                    equipItem.EquipPoints = currentTotalEquipPoint;
                    Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint, connection);
                    UpdateCharacterItem(client, equipItemUID, equipItem, charid, updateCharacterItemNtc, currentEquipInfo, connection);
                    res = CreateUpgradeResponse(equipItemUID, gearUpgradeID, gradeupList, addEquipPoint, currentTotalEquipPoint, (uint)upgradableStatus, goldRequired, isGreatSuccess,
                        currentEquipInfo, equipItem.ItemId, canContinue, dummydata);

                    var newItem = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItem.ItemId);
                    queue.AddRange(Server.AchievementManager.HandleEnhanceItem(client, newItem, connection));
                }
                else
                {
                    equipItem.ItemId = equipItem.ItemId;
                    equipItem.EquipPoints = currentTotalEquipPoint;
                    Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint, connection);
                    res = CreateEquipPointResponse(equipItemUID, addEquipPoint, currentTotalEquipPoint, goldRequired, isGreatSuccess, currentEquipInfo, canContinue, dummydata);
                }

                if (CraftManager.CanPawnExpUp(leadPawn))
                {
                    double BonusExpMultiplier = Server.GpCourseManager.PawnCraftBonus();
                    client.Enqueue(CraftManager.HandlePawnExpUpNtc(client, leadPawn, pawnExp, BonusExpMultiplier), queue);
                    if (CraftManager.CanPawnRankUp(leadPawn))
                    {
                        client.Enqueue(CraftManager.HandlePawnRankUpNtc(client, leadPawn), queue);
                        queue.AddRange(Server.AchievementManager.HandlePawnCrafting(client, leadPawn, connection));
                    }
                }
                else
                {
                    client.Enqueue(CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0), queue);
                }

                Server.Database.UpdatePawnBaseInfo(leadPawn, connection);
            });

            client.Enqueue(updateCharacterItemNtc, queue);
            client.Enqueue(res, queue);
            return queue;
        }

        private void UpdateCharacterItem(GameClient client, string equipItemUID, Item equipItem, uint charid, S2CItemUpdateCharacterItemNtc updateCharacterItemNtc,
            CDataCurrentEquipInfo CurrentEquipInfo, DbConnection? connectionIn = null)
        {
            var (storageType, foundItem) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            if (foundItem != null)
            {
                var (slotno, item, itemnum) = foundItem;
                CharacterCommon characterCommon = null;
                if (storageType == StorageType.CharacterEquipment || storageType == StorageType.PawnEquipment)
                {
                    CurrentEquipInfo.EquipSlot.EquipSlotNo = EquipManager.DetermineEquipSlot(slotno);
                    CurrentEquipInfo.EquipSlot.EquipType = EquipManager.GetEquipTypeFromSlotNo(slotno);
                }

                if (storageType == StorageType.PawnEquipment)
                {
                    uint pawnId = Storages.DeterminePawnId(client.Character, storageType, slotno);
                    CurrentEquipInfo.EquipSlot.PawnId = pawnId;
                    characterCommon = client.Character.Pawns.SingleOrDefault(x => x.PawnId == pawnId);
                }
                else if (storageType == StorageType.CharacterEquipment)
                {
                    CurrentEquipInfo.EquipSlot.CharacterId = charid;
                    characterCommon = client.Character;
                }

                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 0, 0));

                _itemManager.UpgradeStorageItem(Server, client, charid, storageType, equipItem, slotno, connectionIn);
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 1, 1));
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
            }
        }

        private S2CCraftStartEquipGradeUpRes CreateUpgradeResponse(string equipItemUID, uint gradeUpItemID, List<CDataCommonU32> gradeupList,
            uint addEquipPoint, uint currentTotalEquipPoint, uint canUpgrade, uint gold, bool isGreatSuccess, CDataCurrentEquipInfo currentEquip,
            uint beforeItemID, bool upgradable, CDataCraftStartEquipGradeUpUnk0 unk1)
        {
            return new S2CCraftStartEquipGradeUpRes
            {
                GradeUpItemUID = equipItemUID,
                GradeUpItemID = gradeUpItemID, // This has to be the last ID found in gradeupList or it will continue grading up into it.
                GradeUpItemIDList = gradeupList, // Only assign this when its meant to become the next item, or it will autofill the gauge everytime.
                AddEquipPoint = addEquipPoint,
                TotalEquipPoint = currentTotalEquipPoint,
                EquipGrade = canUpgrade,
                Gold = gold,
                IsGreatSuccess = isGreatSuccess,
                CurrentEquip = currentEquip,
                BeforeItemID = beforeItemID,
                Upgradable = upgradable,
                Unk1 = unk1 // Dragon Augment related I guess?
            };
        }

        private S2CCraftStartEquipGradeUpRes CreateEquipPointResponse(string equipItemUID, uint addEquipPoint, uint totalEquipPoint, uint gold, bool isGreatSuccess,
            CDataCurrentEquipInfo currentEquip, bool upgradable, CDataCraftStartEquipGradeUpUnk0 unk1)
        {
            return new S2CCraftStartEquipGradeUpRes
            {
                GradeUpItemUID = equipItemUID,
                AddEquipPoint = addEquipPoint,
                TotalEquipPoint = totalEquipPoint,
                Gold = gold,
                IsGreatSuccess = isGreatSuccess,
                CurrentEquip = currentEquip,
                Upgradable = upgradable,
                Unk1 = unk1 // Dragon Augment related I guess?
            };
        }

        private List<CDataMDataCraftGradeupRecipe> FindRecipeFamily(CDataMDataCraftGradeupRecipe startingRecipe)
        {
            List<CDataMDataCraftGradeupRecipe> recipeFamily = new List<CDataMDataCraftGradeupRecipe>();
            recipeFamily.Add(startingRecipe);
            CDataMDataCraftGradeupRecipe? node = startingRecipe;
            while (node is not null)
            {
                node = Server.AssetRepository.CraftingGradeUpRecipesAsset
                    .SelectMany(recipes => recipes.RecipeList)
                    .Where(x => x.ItemID == node.GradeupItemID)
                    .FirstOrDefault();

                if (node is not null)
                {
                    recipeFamily.Add(node);
                }
            }
            recipeFamily = recipeFamily.OrderBy(x => x.RecipeID).ToList();
            //recipeFamily.ForEach(x => Logger.Debug($"Found recipe family: {startingRecipe.RecipeID} -> {x.RecipeID}"));
            return recipeFamily;
        }
    }
}
