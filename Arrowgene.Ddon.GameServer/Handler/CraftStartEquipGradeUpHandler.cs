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
using Arrowgene.Ddon.Database;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartEquipGradeUpHandler : GameStructurePacketHandler<C2SCraftStartEquipGradeUpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipGradeUpHandler));
        private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };
        private static readonly List<StorageType> StorageEquipNBox = new List<StorageType> {
            StorageType.ItemBagEquipment, StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };

        private readonly ItemManager _itemManager;
        private readonly EquipManager _equipManager;
        private readonly Random _random;

        public CraftStartEquipGradeUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
            _equipManager = Server.EquipManager;
            _random = new Random();
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartEquipGradeUpReq> packet)
        {
            Character common = client.Character;
            string equipItemUID = packet.Structure.EquipItemUID;
            uint equipItemID = _itemManager.LookupItemByUID(Server, equipItemUID);
            ushort equipslot = 0;
            byte equiptype = 0;
            uint charid = client.Character.CharacterId;
            uint pawnid = packet.Structure.CraftMainPawnID;
            var equipItem = Server.Database.SelectItem(equipItemUID);
            byte currentPlusValue = equipItem.PlusValue;

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
            var res = new S2CCraftStartEquipGradeUpRes();
            S2CContextGetLobbyPlayerContextNtc lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;


            // S2CItemUpdateCharacterItemNtc updateCharacterItemNtc2 = new S2CItemUpdateCharacterItemNtc();
            // updateCharacterItemNtc2.UpdateType = ItemNoticeType.ResetCraftpoint;
            // Potentially needed to reset the equippoints properly, but on some quick testing didn't seem to do anything.


            // Handles adding EquipPoints.
            if(dogreatsuccess == true)
            {
                addEquipPoint = 100;
                currentTotalEquipPoint += addEquipPoint;
                bool updateSuccessful = Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
            }
            else
            {
                addEquipPoint = 30;
                currentTotalEquipPoint += addEquipPoint;
                bool updateSuccessful = Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
            }

            // TODO: we need to implement Pawn craft levels since that affects the points that get added
            // TODO: Figure out why the result infobox shows the original/previous step instead of the current/next (potentially related to how we use UIDs)

            // Removes crafting materials
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
                    client.Send(new S2CCraftStartEquipGradeUpRes()
                    {
                        Result = 1
                    });
                    return;
                }
            }

                // TODO: Figure out if you can upgrade several times in a single interaction and if so, handle that lol
                List<CDataCommonU32> gradeuplist = new List<CDataCommonU32>()
                {
                    new CDataCommonU32(gearupgradeID)
                };

                // Subtract less Gold if supportpawn is used.
                if(packet.Structure.CraftSupportPawnIDList.Count > 0)
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
            int requiredPoints = 0;

            if (currentStars == 0)
            {
                requiredPoints = 350;
            }
            else if (currentStars == 1)
            {
                requiredPoints = 700;
            }
            else if (currentStars == 2)
            {
                requiredPoints = 1000;
            }
            else if (currentStars == 3)
            {
                requiredPoints = 1500;
                canContinue = false;
            }
            // else if (currentStars == 4)
            // {
            //     requiredPoints = 800;
            // }
            // TODO: Need to run a check on items that can become "TRUE" which only requires 800 points.

            // Handling the comparison to permit upgrading or not.
            if (currentTotalEquipPoint >= requiredPoints)
            {
                canUpgrade = true;
                addEquipPoint = 0;
                currentTotalEquipPoint = 0;
                bool updateSuccessful = Server.Database.UpdateItemEquipPoints(equipItemUID, currentTotalEquipPoint);
            }
            

            // More dummy data, looks like its dragonfroce related.
            CDataCraftStartEquipGradeUpUnk0Unk0 DragonForceData = new CDataCraftStartEquipGradeUpUnk0Unk0()
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
                Unk0 = new List<CDataCraftStartEquipGradeUpUnk0Unk0> { DragonForceData },
                Unk1 = 0,
                Unk2 = 0,
                Unk3 = 0,               // No idea what these 3 bytes are for
                DragonFroce = false,    // makes the Dragonforce slot popup appear if set to true.
            };
            // TODO: Source these values accurately when we know what they are. ^

            Item UpgradedItem = new Item()
            {
                ItemId = gearupgradeID,
                Unk3 = equipItem.Unk3,   // Safety setting,
                Color = equipItem.Color,
                PlusValue = currentPlusValue,
                EquipPoints = 0,
                WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                AddStatusData = equipItem.AddStatusData,
                EquipElementParamList = new List<CDataEquipElementParam>()
            };

            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
                CharId = charid,
                PawnId = pawnid,
                EquipType = equiptype,
                EquipSlot = equipslot,
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUID = equipItemUID,
                EquipSlot = EquipmentSlot
            };

            bool isEquipped = _equipManager.IsItemEquipped(common, equipItemUID);
            if (canUpgrade)
            {
                if (isEquipped)
                {
                    List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Performance)
                        .Union(common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Visual))
                        .ToList();

                    var equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);
                    equipslot = equipInfo.EquipCategory;
                    equiptype = equipInfo.EquipType;

                    _equipManager.ReplaceEquippedItem(Server, client, common, StorageType.Unk14, equipItemUID, UpgradedItem.UId, gearupgradeID, UpgradedItem, (EquipType)equiptype, (byte)equipslot);

                    lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                    GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, client.Character);
                    client.Send(lobbyPlayerContextNtc);
                }
                else
                {
                    StorageType storageType = FindItemByUID(common, equipItemUID).StorageType ?? throw new Exception("Item not found in any storage type");
                    ushort slotno = 0;
                    uint itemnum = 0;
                    Item item;
                    var foundItem = common.Storage.getStorage(StorageType.ItemBagEquipment).findItemByUId(equipItemUID);

                    switch (storageType)
                    {
                        case StorageType.ItemBagEquipment:
                            foundItem = common.Storage.getStorage(StorageType.ItemBagEquipment).findItemByUId(equipItemUID);
                            break;
                        case StorageType.StorageBoxNormal:
                            foundItem = common.Storage.getStorage(StorageType.StorageBoxNormal).findItemByUId(equipItemUID);
                            break;
                        case StorageType.StorageBoxExpansion:
                            foundItem = common.Storage.getStorage(StorageType.StorageBoxExpansion).findItemByUId(equipItemUID);
                            break;
                        case StorageType.StorageChest:
                            foundItem = common.Storage.getStorage(StorageType.StorageChest).findItemByUId(equipItemUID);
                            break;
                        default:
                            Logger.Debug($"Bruh this found an item in {storageType}, not cool.");
                            break;
                    }

                    if (foundItem != null)
                    {
                        (slotno, item, itemnum) = foundItem;
                        _itemManager.ReplaceStorageItem(
                            Server,
                            client,
                            common,
                            charid,
                            storageType,
                            UpgradedItem,
                            (byte)slotno
                        );
                        Logger.Debug($"Your Slot is: {slotno}, in {storageType} hopefully thats right?");
                    }
                    else
                    {
                        Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                    }
                        res = new S2CCraftStartEquipGradeUpRes()
                        {
                            GradeUpItemUID = UpgradedItem.UId, // Should be the UID for the gradeupitem. (which after the UID rework will just be the same as the original item)
                            GradeUpItemID = gearupgradeID, // This has to be the upgrade step ID.
                            GradeUpItemIDList = gradeuplist, // This list should start with the next ID.
                            AddEquipPoint = 0,
                            TotalEquipPoint = 0,
                            EquipGrade = EquipRank,
                            Gold = goldRequired,
                            IsGreatSuccess = dogreatsuccess,
                            //CurrentEquip = CurrentEquipInfo,   
                            BeforeItemID = equipItemID,
                            Upgradable = canContinue,
                            Unk1 = dummydata
                        };

                    List<CDataItemUpdateResult> updateResults = Server.ItemManager.ReplaceStorageItem(Server, client, common, charid, storageType, UpgradedItem, (byte)slotno);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                
                };
            };

            // If GradeUpItemID & UID are populated it will auto fill the bar, I guess I need an if check somewhere to decide if we send that data or not. 
            if(!canUpgrade)
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
                    Unk1 = dummydata // I think this is to track slotted crests, dyes, etc
                };

            };
            client.Send(updateCharacterItemNtc);
            lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
            GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, client.Character);
            client.Send(lobbyPlayerContextNtc);
            client.Send(res);
        }
        private (StorageType? StorageType, (ushort SlotNo, Item Item, uint ItemNum)?) FindItemByUID(Character character, string itemUID)
        {
            foreach (var storageType in STORAGE_TYPES)
            {
                var foundItem = character.Storage.getStorage(storageType).findItemByUId(itemUID);
                if (foundItem != null)
                {
                    return (storageType, (foundItem.Item1, foundItem.Item2, foundItem.Item3));
                }
            }
            return (null, null);
        }
    }
}