#nullable enable
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartCraftHandler : GameRequestPacketHandler<C2SCraftStartCraftReq, S2CCraftStartCraftRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartCraftHandler));

        private static readonly HashSet<ItemSubCategory> BannedSubCategories = new()
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
            CDataMDataCraftRecipe recipe = Server.AssetRepository.CraftingRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Single(recipe => recipe.RecipeID == request.RecipeID);
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

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = ItemNoticeType.StartCraft;

            // Remove crafting materials
            foreach (var craftMaterial in request.CraftMaterialList)
            {
                try
                {
                    List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes,
                        craftMaterial.ItemUId, craftMaterial.ItemNum);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
                catch (NotEnoughItemsException e)
                {
                    Logger.Exception(e);
                    return new S2CCraftStartCraftRes()
                    {
                        Result = 1
                    };
                }
            }

            Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);
            List<Pawn> pawns = new List<Pawn> { leadPawn };
            pawns.AddRange(request.CraftSupportPawnIDList.Select(p => Server.CraftManager.FindPawn(client, p.PawnId)));
            List<uint> productionSpeedLevels = new List<uint>();
            List<uint> consumableQuantityLevels = new List<uint>();
            List<uint> costPerformanceLevels = new List<uint>();
            List<uint> qualityLevels = new List<uint>();
            foreach (Pawn pawn in pawns)
            {
                if (pawn != null)
                {
                    productionSpeedLevels.Add(CraftManager.GetPawnProductionSpeedLevel(pawn));
                    consumableQuantityLevels.Add(CraftManager.GetPawnConsumableQuantityLevel(pawn));
                    costPerformanceLevels.Add(CraftManager.GetPawnCostPerformanceLevel(pawn));
                    qualityLevels.Add(CraftManager.GetPawnEquipmentQualityLevel(pawn));
                }
                else
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID, "Couldn't find the Pawn ID.");
                }
            }

            double calculatedOdds = CraftManager.CalculateEquipmentQualityIncreaseRate(qualityLevels);
            uint plusValue = 0;
            bool isGreatSuccessEquipmentQuality = false;
            bool canPlusValue = !itemInfo.SubCategory.HasValue || !BannedSubCategories.Contains(itemInfo.SubCategory.Value);
            if (canPlusValue && !string.IsNullOrEmpty(request.RefineMaterialUID))
            {
                Item refineMaterialItem = Server.Database.SelectStorageItemByUId(request.RefineMaterialUID);
                CraftCalculationResult craftCalculationResult = CraftManager.CalculateEquipmentQuality(refineMaterialItem, (uint)calculatedOdds);
                plusValue = craftCalculationResult.CalculatedValue;
                isGreatSuccessEquipmentQuality = craftCalculationResult.IsGreatSuccess;

                try
                {
                    List<CDataItemUpdateResult> updateResults =
                        Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, request.RefineMaterialUID, 1);
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
                CraftCalculationResult craftCalculationResult = CraftManager.CalculateConsumableQuantity(consumableQuantityLevels, (uint)calculatedOdds);
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
                RemainTime = Server.CraftManager.CalculateRecipeProductionSpeed(recipe.Time, productionSpeedLevels),
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

            Server.Database.InsertPawnCraftProgress(craftProgress);

            // Subtract craft price
            CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold,
                            Server.CraftManager.CalculateRecipeCost(recipe.Cost, costPerformanceLevels) * request.CreateCount);
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

            client.Send(updateCharacterItemNtc);
            return new S2CCraftStartCraftRes();
        }
    }
}
