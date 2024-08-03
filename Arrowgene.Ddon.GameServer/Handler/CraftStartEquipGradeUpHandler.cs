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
        private static readonly List<StorageType> StorageEquipNBox = new List<StorageType> {
            StorageType.ItemBagEquipment, StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest, StorageType.CharacterEquipment, StorageType.PawnEquipment
        };

        private readonly ItemManager _itemManager;
        private readonly Random _random;

        public CraftStartEquipGradeUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
            _random = Random.Shared;
        }

        public override S2CCraftStartEquipGradeUpRes Handle(GameClient client, C2SCraftStartEquipGradeUpReq request)

        {
            #region Initializing vars
            Character common = client.Character;
            string equipItemUID = request.EquipItemUID;
            Item equipItem = Server.Database.SelectStorageItemByUId(equipItemUID);
            uint equipItemID = _itemManager.LookupItemByUID(Server, equipItemUID);
            uint charid = client.Character.CharacterId;
            uint pawnid = request.CraftMainPawnID;

            CDataMDataCraftGradeupRecipe json_data = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => recipe.ItemID == equipItemID)
                .First();

            uint gearupgradeID = json_data.GradeupItemID;
            uint goldRequired = json_data.Cost;
            uint EquipRank = json_data.Unk0;
            bool canContinue = true;
            uint currentTotalEquipPoint = equipItem.EquipPoints;
            uint previousTotalEquipPoint = currentTotalEquipPoint;
            uint addEquipPoint = 0;     
            bool dogreatsuccess = _random.Next(5) == 0; // 1 in 5 chance to be true, someone said it was 20%.
            bool canUpgrade = false;

            double minMultiplier = 0.8;
            double maxMultiplier = 1.2;
            double pointsMultiplier = minMultiplier + (_random.NextDouble() * (maxMultiplier - minMultiplier));

            List<CDataItemUpdateResult> updateResults;
            var res = new S2CCraftStartEquipGradeUpRes();
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

            #endregion

            #region Point Distribution 
            // Handles adding EquipPoints.
            if(dogreatsuccess == true)
            {
                addEquipPoint = 300;
                addEquipPoint = (uint)(addEquipPoint * pointsMultiplier);
                currentTotalEquipPoint += addEquipPoint;
                bool updateSuccessful = Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
            }
            else
            {
                addEquipPoint = 180;
                addEquipPoint = (uint)(addEquipPoint * pointsMultiplier);
                currentTotalEquipPoint += addEquipPoint;
                bool updateSuccessful = Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
            }
            #endregion

            // TODO: we need to implement Pawn craft levels since that affects the points that get added

            // Removes crafting materials
            foreach (var craftMaterial in request.CraftMaterialList)
            {
                    updateResults = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, craftMaterial.ItemUId, craftMaterial.ItemNum);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
            }

                // TODO: Figure out if you can upgrade several times in a single interaction and if so, handle that lol
                // 29/07/24, seems like points should be set to 0 upon upgrading, so this probably never allows more than one?
                List<CDataCommonU32> gradeuplist = new List<CDataCommonU32>()
                {
                    new CDataCommonU32(gearupgradeID)
                };

                // Subtract less Gold if supportpawn is used.
                if(request.CraftSupportPawnIDList.Count > 0)
                {
                    goldRequired = (uint)(goldRequired*0.95);
                    currentTotalEquipPoint += 10;
                }

                // Substract Gold based on JSON cost.
                CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, goldRequired);
                updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);


            // Handling the check on how many points we need to upgrade the weapon in its current state.
            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItemID);
            byte currentStars = (byte)itemInfo.Quality;
            int requiredPoints = currentStars switch
            {
                0 => 350,
                1 => 700,
                2 => 1000,
                3 => 1500,
                4 => 800,
                _ => throw new InvalidOperationException("Invalid star level")
            };

            // TODO: Check if an item can reach "True" and handle the UI properly here
            if (currentStars == 3) canContinue = false;

            // Updating the item.
            equipItem.ItemId = gearupgradeID;
            equipItem.Unk3 = equipItem.Unk3;
            equipItem.Color = equipItem.Color;
            equipItem.PlusValue = equipItem.PlusValue;
            equipItem.EquipPoints = equipItem.EquipPoints;
            equipItem.WeaponCrestDataList = equipItem.WeaponCrestDataList;
            equipItem.AddStatusData = equipItem.AddStatusData;
            equipItem.EquipElementParamList = equipItem.EquipElementParamList;

            // Handling the comparison to permit upgrading or not.
            if (currentTotalEquipPoint >= requiredPoints)
            {
                canUpgrade = true;
                addEquipPoint = 0;
                currentTotalEquipPoint = 0;
                equipItem.EquipPoints = 0;
                bool updateSuccessful = Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
            }
            

            // More dummy data, looks like its dragonfroce related.
            CDataCraftStartEquipGradeUpUnk0Unk0 DragonAugmentData = new CDataCraftStartEquipGradeUpUnk0Unk0()
            {
                Unk0 = 1,          // Probably Dragon Force related.
                Unk1 = 0,
                Unk2 = 0,          // setting this to a value above 0 seems to stop displaying "UP" ?
                Unk3 = 1,          // displays "UP" next to the DF upon succesful enhance.
                IsMax = false,      // displays Max on the DF popup.
            };

            // Dummy data for Unk1.
            CDataCraftStartEquipGradeUpUnk0 dummydata = new CDataCraftStartEquipGradeUpUnk0()
            {
                Unk0 = new List<CDataCraftStartEquipGradeUpUnk0Unk0> { DragonAugmentData },
                Unk1 = 0,
                Unk2 = 0,
                Unk3 = 0,               // No idea what these 3 bytes are for
                DragonAugment = false,    // makes the DragonAugment slot popup appear if set to true.
            };
            // TODO: Source these values accurately when we know what they are. ^




            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
                CharacterId = charid,
                PawnId = pawnid,
                EquipType = 0,
                EquipSlotNo = 0
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
                EquipSlot = EquipmentSlot
            };


            if (canUpgrade)
            {
                StorageType storageType = FindItemByUID(common, equipItemUID).StorageType ?? throw new Exception("Item not found in any storage type");
                ushort slotno = 0;
                uint itemnum = 0;
                Item item;
                var foundItem = common.Storage.GetStorage(StorageType.ItemBagEquipment).FindItemByUId(equipItemUID);

                switch (storageType)
                {
                    case StorageType.CharacterEquipment:
                        foundItem = common.Storage.GetStorage(StorageType.CharacterEquipment).FindItemByUId(equipItemUID);
                        List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.AsCDataCharacterEquipInfo(EquipType.Performance)
                                    .Union(common.Equipment.AsCDataCharacterEquipInfo(EquipType.Visual))
                                    .ToList();

                            var equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);
                            EquipmentSlot.EquipSlotNo = equipInfo.EquipCategory;
                            EquipmentSlot.EquipType = equipInfo.EquipType;
                        break;

                    case StorageType.PawnEquipment:
                        foundItem = common.Storage.GetStorage(StorageType.PawnEquipment).FindItemByUId(equipItemUID);
                        characterEquipList = common.Equipment.AsCDataCharacterEquipInfo(EquipType.Performance)
                                    .Union(common.Equipment.AsCDataCharacterEquipInfo(EquipType.Visual))
                                    .ToList();

                            equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);
                            EquipmentSlot.EquipSlotNo = equipInfo.EquipCategory;
                            EquipmentSlot.EquipType = equipInfo.EquipType;
                        break;

                    case StorageType.ItemBagEquipment:
                        foundItem = common.Storage.GetStorage(StorageType.ItemBagEquipment).FindItemByUId(equipItemUID);
                        break;
                    case StorageType.StorageBoxNormal:
                        foundItem = common.Storage.GetStorage(StorageType.StorageBoxNormal).FindItemByUId(equipItemUID);
                        break;
                    case StorageType.StorageBoxExpansion:
                        foundItem = common.Storage.GetStorage(StorageType.StorageBoxExpansion).FindItemByUId(equipItemUID);
                        break;
                    case StorageType.StorageChest:
                        foundItem = common.Storage.GetStorage(StorageType.StorageChest).FindItemByUId(equipItemUID);
                        break;
                    default:
                        Logger.Error($"Item found in {storageType}. Which isn't supported.");
                        break;
                }
                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(common, equipItem, storageType, (byte)slotno, 0, 0));
                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    updateResults = _itemManager.UpgradeStorageItem(
                        Server,
                        client,
                        common,
                        charid,
                        storageType,
                        equipItem,
                        (byte)slotno
                    );
                    Logger.Debug($"Your Slot is: {slotno}, in {storageType} for UID {equipItem.UId}.");

                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(common, equipItem, storageType, (byte)slotno, 1, 1));
                    client.Send(updateCharacterItemNtc);
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                }

                if (storageType == StorageType.PawnEquipment)
                {
                    CurrentEquipInfo.EquipSlot.CharacterId = 0;
                }
                else
                {
                    CurrentEquipInfo.EquipSlot.PawnId = 0;
                }
                
                res = new S2CCraftStartEquipGradeUpRes()
                {
                    GradeUpItemUID = equipItemUID,
                    GradeUpItemID = equipItem.ItemId,
                    GradeUpItemIDList = gradeuplist, // Only assign this when its meant to become the next item, or it will autofill the gauge everytime.
                    AddEquipPoint = 0,
                    TotalEquipPoint = 0,
                    EquipGrade = EquipRank, // Unclear why the client wants this? as long as its a number it doesn't seem to matter waht you set it
                    Gold = goldRequired,
                    IsGreatSuccess = dogreatsuccess,
                    CurrentEquip = CurrentEquipInfo,   
                    BeforeItemID = equipItemID,
                    Upgradable = canContinue,
                    Unk1 = dummydata // Dragon Augment related I guess?
                };
            }
            else
            {
                res = new S2CCraftStartEquipGradeUpRes()
                {
                    GradeUpItemUID = equipItemUID,
                    AddEquipPoint = addEquipPoint,
                    TotalEquipPoint = currentTotalEquipPoint,
                    Gold = goldRequired,
                    IsGreatSuccess = dogreatsuccess,
                    CurrentEquip = CurrentEquipInfo,
                    Upgradable = canContinue,
                    Unk1 = dummydata // Dragon Augment related I guess?
                };

            };
            return res;
        }
        private (StorageType? StorageType, (ushort SlotNo, Item Item, uint ItemNum)?) FindItemByUID(Character character, string itemUID)
        {
            foreach (var storageType in StorageEquipNBox)
            {
                var foundItem = character.Storage.GetStorage(storageType).FindItemByUId(itemUID);
                if (foundItem != null)
                {
                    return (storageType, (foundItem.Item1, foundItem.Item2, foundItem.Item3));
                }
            }
            return (null, null);
        }
    }
}