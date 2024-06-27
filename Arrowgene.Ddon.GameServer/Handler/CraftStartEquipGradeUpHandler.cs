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
            uint equipItemID = _itemManager.LookupItemByUID(Server, equipItemUID); // Finding the Recipe we need based on the requested UID. 
            ushort equipslot = 0;
            byte equiptype = 0;
            uint charid = client.Character.CharacterId;
            uint pawnid = packet.Structure.CraftMainPawnID;
            var equipItem = Server.Database.SelectItem(equipItemUID);
            byte currentPlusValue = equipItem.PlusValue;

            // Getting access to the GradeUpRecipe JSON data.
            CDataMDataCraftGradeupRecipe json_data = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => recipe.ItemID == equipItemID)
                .First();

            // Define local variables for calculations
            uint gearupgradeID = json_data.GradeupItemID;
            uint goldRequired = json_data.Cost;
            uint nextGrade = json_data.Unk0; // This might be Unk0 in the JSON but is probably in the DB or something.
            bool canContinue = json_data.Unk1;
            uint currentTotalEquipPoint = 10; // Equip Points are probably handled elsewhere, since its not in the JSON or Request.
            uint addEquipPoint = 0;     
            bool dogreatsuccess = _random.Next(5) == 0; // 1 in 5 chance to be true, someone said it was 20%.
            

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

            if(dogreatsuccess == true)
            {

                addEquipPoint = 100;
            }
            else
            {
                addEquipPoint = 30;
            }
            // TODO: Figure out why the bar always fills fully regardless of fed numbers? it wasn't doing this earlier on.
            // TODO: we need to track the EquipPoints in the DB, and anywhere we set it to 0 it should pull the correct points from the DB instead.
            // TODO: we need to implement Pawn craft levels since that affects the points that get added
            // TODO: Figure out why the result infobox shows the original/previous step instead of the current/next

            // Remove crafting materials
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


            // This list should contain the next ID you will go into, if you happen to have enough points to go into multiple we must include those too.
            // For now I just hardcode the next ID without accounting for the potential for being able to do it multiple times.
            // TODO: I guess figure this out lol
            List<CDataCommonU32> gradeuplist = new List<CDataCommonU32>()
            {
                new CDataCommonU32(gearupgradeID)
            };


            // More dummy data
            CDataCraftStartEquipGradeUpUnk0Unk0 internaldummydata = new CDataCraftStartEquipGradeUpUnk0Unk0()
            {
                Unk0 = 1,          // Probably Dragon Force related.
                Unk1 = 0,
                Unk2 = 0,          // setting this to a value above 0 seems to stop displaying "UP" ?
                Unk3 = 1,          // displays "UP" next to the DF upon succesful enhance. This appears to be dragon force related.
                Unk4 = false,      // displays Max on the DF popup. This appears to be dragon force related.
            };

            // Dummy data for Unk1.
            CDataCraftStartEquipGradeUpUnk0 dummydata = new CDataCraftStartEquipGradeUpUnk0()
            {
                Unk0 = new List<CDataCraftStartEquipGradeUpUnk0Unk0> { internaldummydata },
                Unk1 = 0,
                Unk2 = 0,
                Unk3 = 0,
                Unk4 = false,    // makes the Dragonforce slot popup appear if set to true.
            };
            // TODO: Source these values accurately when we know what they are. ^

            // Subtract less Gold if supportpawn is used.
            if(packet.Structure.CraftSupportPawnIDList.Count > 0)
            {
                goldRequired = (uint)(goldRequired*0.95);
            }

            // Substract Gold based on JSON cost.
            CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, goldRequired);
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

            // TODO: Figure out how to stop the server thinking an item is equipped just because it has same UID as a non-equipped gear you're trying to upgrade.

            List<CDataItemUpdateResult> AddItemResult;
            List<CDataItemUpdateResult> RemoveItemResult;

            Item UpgradedItem = new Item()
            {
                ItemId = gearupgradeID,
                Unk3 = 0,   // Safety setting,
                Color = 0,
                PlusValue = currentPlusValue,
                WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                ArmorCrestDataList = new List<CDataArmorCrestData>(),
                EquipElementParamList = new List<CDataEquipElementParam>()
            };

            bool isEquipped = _equipManager.IsItemEquipped(common, equipItemUID);
            if (isEquipped)
            {
                List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Performance)
                    .Union(common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Visual))
                    .ToList();

                var equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);
                equipslot = equipInfo.EquipCategory;
                equiptype = equipInfo.EquipType;

                _equipManager.ReplaceEquippedItem(Server, client, common, StorageType.Unk14, equipItemUID, UpgradedItem.UId, gearupgradeID, UpgradedItem, (EquipType)equiptype, (byte)equipslot);

                updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                    ItemList = new CDataItemList() {
                        ItemUId = UpgradedItem.UId,
                        ItemId = UpgradedItem.ItemId,
                        ItemNum = 0,
                        Unk3 = UpgradedItem.Unk3,
                        StorageType = StorageType.Unk14,
                        SlotNo = 1,
                        Color = UpgradedItem.Color,
                        PlusValue = UpgradedItem.PlusValue,
                        Bind = true,
                        EquipPoint = 0,
                        EquipCharacterID = charid,
                        EquipPawnID = 0,
                        WeaponCrestDataList = UpgradedItem.WeaponCrestDataList,
                        ArmorCrestDataList = UpgradedItem.ArmorCrestDataList,
                        EquipElementParamList = UpgradedItem.EquipElementParamList
                    },
                    UpdateItemNum = 1,
                });    
                // TODO: Figure out how to update the NTC without crashing the client, confirmed possible by this
                // S2CContextGetLobbyPlayerContextNtc lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                // GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, client.Character);
                // client.Send(lobbyPlayerContextNtc);
                // only drawback is that this drops PlusValue and will probably drop a lot of data, like color/crests etc.
            }
            else
            {
                RemoveItemResult = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, equipItemUID, 1);
                bool isBagItem =! RemoveItemResult.Any(result => result.ItemList.StorageType == StorageType.StorageBoxNormal ||
                                                                result.ItemList.StorageType == StorageType.StorageBoxExpansion);
                updateCharacterItemNtc.UpdateItemList.AddRange(RemoveItemResult);
                AddItemResult = _itemManager.AddItem(Server, client.Character, isBagItem, gearupgradeID, 1, currentPlusValue);
                updateCharacterItemNtc.UpdateItemList.AddRange(AddItemResult);
                //TODO: Adapt the Equipped code here, since that behaves in a more intended way.
             };
            
            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
                Unk0 = charid,
                Unk1 = pawnid,
                Unk2 = equiptype, // type
                Unk3 = equipslot, // slot
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUID = equipItemUID,
                EquipSlot = EquipmentSlot
            };

            // Supplying the response packet with data
            var res = new S2CCraftStartEquipGradeUpRes()
            {
                GradeUpItemUID = UpgradedItem.UId, // Should be the UID for the gradeupitem.
                GradeUpItemID = gearupgradeID, // This has to be the upgrade step ID.
                GradeUpItemIDList = gradeuplist, // This list should start with the next ID.
                AddEquipPoint = addEquipPoint,
                TotalEquipPoint = currentTotalEquipPoint + addEquipPoint,
                EquipGrade = nextGrade, // It expects a valid number or it won't show the result when you enhance, (presumably we give this value when filling the bar)
                Gold = goldRequired, // No noticable difference when supplying this info, but it wants it so whatever.
                IsGreatSuccess = dogreatsuccess, // Just changes the banner from "Success" to "GreatSuccess" we'd have to augment the addEquipPoint value when this is true.
                CurrentEquip = CurrentEquipInfo, // Client uses this to determine whats equipped to show the Arisen sign in the menu.              
                BeforeItemID = equipItemID, // I don't know why the response wants the "beforeid" its unclear what this means too? should it be 0 if step 1? hmm.
                Unk0 = canContinue, // If True it says "Gradeu Up" if False it says "Grade Max"
                Unk1 = dummydata // I think this is to track slotted crests, dyes, etc
            };
            client.Send(updateCharacterItemNtc);
            client.Send(res);
        }
    }
}