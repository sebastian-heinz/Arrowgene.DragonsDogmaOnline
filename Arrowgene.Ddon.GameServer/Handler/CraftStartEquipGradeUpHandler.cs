#nullable enable
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartEquipGradeUpHandler : GameRequestPacketHandler<C2SCraftStartEquipGradeUpReq, S2CCraftStartEquipGradeUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipGradeUpHandler));
        private readonly ItemManager _itemManager;

        public CraftStartEquipGradeUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
        }

        public override S2CCraftStartEquipGradeUpRes Handle(GameClient client, C2SCraftStartEquipGradeUpReq request)
        {
            string equipItemUID = request.EquipItemUID;
            Character character = client.Character;
            var ramItem = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            var equipItem = ramItem.Item2.Item2;
            uint charid = client.Character.CharacterId;
            uint craftpawnid = request.CraftMainPawnID;

            // Fetch the crafting recipe data for the item
            CDataMDataCraftGradeupRecipe json_data = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .First(recipe => recipe.ItemID == equipItem.ItemId);

            uint gearupgradeID = json_data.GradeupItemID;
            uint goldRequired = json_data.Cost;
            uint EquipRank = json_data.Unk0;
            uint PawnExp = json_data.Exp;
            bool canContinue = true;
            bool IsGreatSuccess = Random.Shared.Next(5) == 0;
            uint currentTotalEquipPoint = equipItem.EquipPoints;

            List<CDataCommonU32> gradeuplist = new() { new CDataCommonU32(gearupgradeID) };
            CDataCurrentEquipInfo CurrentEquipInfo = new()
            {
                ItemUId = equipItemUID,
            };

            // More dummy data, looks like its DragonAugment related.
            CDataCraftStartEquipGradeUpUnk0Unk0 DragonAugmentData = new();
            CDataCraftStartEquipGradeUpUnk0 dummydata = new() // TODO: Figure this out
            {
                Unk0 = new List<CDataCraftStartEquipGradeUpUnk0Unk0> { DragonAugmentData },
                DragonAugment = false,    // makes the DragonAugment slot popup appear if set to true.
            };

            var res = new S2CCraftStartEquipGradeUpRes();
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new();


            // Removes crafting materials
            foreach (var craftMaterial in request.CraftMaterialList)
            {
                try
                {
                var updateResults = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, craftMaterial.ItemUId, craftMaterial.ItemNum);
                updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
                catch (NotEnoughItemsException)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has Occurred.");
                }
            }

            // Update item equip points in the database
            uint addEquipPoint = (uint)((IsGreatSuccess ? 300 : 180) * (0.8 + (Random.Shared.NextDouble() * 0.4))); 
            currentTotalEquipPoint += addEquipPoint;

            // Subtract less Gold if support pawn is used and add slightly more points
            if (request.CraftSupportPawnIDList.Count > 0)
            {
                goldRequired = (uint)(goldRequired * 0.95);
                currentTotalEquipPoint += 10;
            }

            var updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, goldRequired);
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

            // Handling the check on how many points we need to upgrade the weapon in its current state.
            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItem.ItemId);
            byte currentStars = (byte)itemInfo.Quality;
            int requiredPoints = currentStars switch
            {
                0 => 350,
                1 => 700,
                2 => 1000,
                3 => 1500,
                4 => 800,  // TODO: Check if the gear can reach "True" level (4) and handle that properly with "canContinue"
                _ => throw new InvalidOperationException("Invalid star level")
            };

            if (currentStars == 3)
            {
                canContinue = false;
            }
            bool DoUpgrade = currentTotalEquipPoint >= requiredPoints;

            if (DoUpgrade)
            {
                equipItem.ItemId = gearupgradeID;
                if (canContinue)
                {
                    currentTotalEquipPoint -= (uint)requiredPoints;
                }
                else
                {
                    currentTotalEquipPoint = 0;
                }
                equipItem.EquipPoints = currentTotalEquipPoint;
                Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
                UpdateCharacterItem(client, equipItemUID, equipItem, charid, updateCharacterItemNtc, CurrentEquipInfo);
                res = CreateUpgradeResponse(equipItemUID, gearupgradeID, gradeuplist, addEquipPoint, currentTotalEquipPoint, EquipRank, goldRequired, IsGreatSuccess, CurrentEquipInfo, equipItem.ItemId, canContinue, dummydata);
            }
            else
            {
                equipItem.ItemId = equipItem.ItemId;
                equipItem.EquipPoints = currentTotalEquipPoint;
                Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
                res = CreateEquipPointResponse(equipItemUID, addEquipPoint, currentTotalEquipPoint, goldRequired, IsGreatSuccess, CurrentEquipInfo, canContinue, dummydata);
            }

            // TODO: Store saved pawn exp
            S2CCraftCraftExpUpNtc expNtc = new S2CCraftCraftExpUpNtc()
            {
                PawnId = request.CraftMainPawnID,
                AddExp = PawnExp,
                ExtraBonusExp = 0,
                TotalExp = PawnExp,
                CraftRankLimit = 0
            };
            client.Send(expNtc);
            client.Send(updateCharacterItemNtc);
            return res;
        }

        private void UpdateCharacterItem(GameClient client, string equipItemUID, Item equipItem, uint charid, S2CItemUpdateCharacterItemNtc updateCharacterItemNtc, CDataCurrentEquipInfo CurrentEquipInfo)
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

                _itemManager.UpgradeStorageItem(Server, client, charid, storageType, equipItem, slotno);
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 1, 1));
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
            }

        }
        private S2CCraftStartEquipGradeUpRes CreateUpgradeResponse(string equipItemUID, uint gradeUpItemID, List<CDataCommonU32> gradeuplist, uint addEquipPoint, uint currentTotalEquipPoint, uint equipGrade, uint gold, bool isGreatSuccess, CDataCurrentEquipInfo currentEquip, uint beforeItemID, bool upgradable, CDataCraftStartEquipGradeUpUnk0 unk1)
        {
            return new S2CCraftStartEquipGradeUpRes
            {
                GradeUpItemUID = equipItemUID,
                GradeUpItemID = gradeUpItemID,
                GradeUpItemIDList = gradeuplist, // Only assign this when its meant to become the next item, or it will autofill the gauge everytime.
                AddEquipPoint = addEquipPoint,
                TotalEquipPoint = currentTotalEquipPoint,
                EquipGrade = equipGrade, // Unclear why the client wants this? as long as its a number it doesn't seem to matter what you set it as.
                Gold = gold,
                IsGreatSuccess = isGreatSuccess,
                CurrentEquip = currentEquip,
                BeforeItemID = beforeItemID,
                Upgradable = upgradable,
                Unk1 = unk1 // Dragon Augment related I guess?
            };
        }
        
        private S2CCraftStartEquipGradeUpRes CreateEquipPointResponse(string equipItemUID, uint addEquipPoint, uint totalEquipPoint, uint gold, bool isGreatSuccess, CDataCurrentEquipInfo currentEquip, bool upgradable, CDataCraftStartEquipGradeUpUnk0 unk1)
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
    }
}
