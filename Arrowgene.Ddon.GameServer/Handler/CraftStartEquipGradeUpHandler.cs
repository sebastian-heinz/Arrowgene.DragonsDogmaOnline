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
using Arrowgene.Ddon.GameServer.Chat.Command.Commands;
using System.Data.Entity;
using Arrowgene.Ddon.Database.Sql;
using System.Runtime.CompilerServices;
using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartEquipGradeUpHandler : GameStructurePacketHandler<C2SCraftStartEquipGradeUpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipGradeUpHandler));
        private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
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

            // Getting access to the GradeUpRecipe JSON data.
            CDataMDataCraftGradeupRecipe json_data = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => recipe.ItemID == equipItemID)
                .First();

            // Define local variables for calculations
            uint gearupgradeID = json_data.GradeupItemID;
            uint goldRequired = json_data.Cost;
            uint nextGrade = json_data.Unk0; // This might be Unk0 in the JSON but is probably in the DB or something.
            uint currentTotalEquipPoint = 0; // Equip Points are probably handled elsewhere, since its not in the JSON or Request.
            uint addEquipPoint = 0;     
            bool dogreatsucess = _random.Next(5) == 0; // 1 in 5 chance to be true, someone said it was 20%.


            if(dogreatsucess == true)
            {
                currentTotalEquipPoint = 500;
            }
            else
            {
                currentTotalEquipPoint = 200;
            }
            // TODO: Figure out why the bar always fills fully regardless of fed numbers? it wasn't doing this earlier on.
            // TODO: we need to implement Pawn craft levels since that affects the points that get added
            // TODO: we need to track the EquipPoints in the DB.
            // TODO: You require atleast 2 pieces of the same gear to complete the Enhance cycle properly, or you don't get the info box after completing and can't
            // enhance it again to the next stage, though you can relog and it will allow you to.

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;


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


            // Made a list of all bronzesword IDs 
            // This list should start from the next gradeup ID from the current, (if you're upgrading a 1674, the list should start as 2697.)
            uint firstid = 62;
            uint secondid = 1674;
            uint thirdid = 2697;
            uint fourthid = 3720;
            uint fifthid = 4743;
            List<CDataCommonU32> dummygradeuplist = new List<CDataCommonU32>()
            {
                new CDataCommonU32(firstid),
                new CDataCommonU32(secondid),
                new CDataCommonU32(thirdid),
                new CDataCommonU32(fourthid),
                new CDataCommonU32(fifthid)
            };


            // More dummy data
            CDataCraftStartEquipGradeUpUnk0Unk0 internaldummydata = new CDataCraftStartEquipGradeUpUnk0Unk0()
            {
                Unk0 = 1,
                Unk1 = 0,  
                Unk2 = 0,
                Unk3 = 0,
                Unk4 = false,      
            };

            // Dummy data for Unk1.
            CDataCraftStartEquipGradeUpUnk0 dummydata = new CDataCraftStartEquipGradeUpUnk0()
            {
                Unk0 = new List<CDataCraftStartEquipGradeUpUnk0Unk0> { internaldummydata },
                Unk1 = 1,
                Unk2 = 0,
                Unk3 = 1,
                Unk4 = false,
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


            // Checking if the Gear is equipped or not first.
            List<CDataItemUpdateResult> AddItemResult;
            List<CDataItemUpdateResult> RemoveItemResult;
            bool isEquipped = _equipManager.IsItemEquipped(common, equipItemUID);
            if (isEquipped)
            {
                List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Performance)
                    .Union(common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Visual))
                    .ToList();

                var equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);

                equipslot = equipInfo.EquipCategory;
                equiptype = equipInfo.EquipType;

                AddItemResult = _itemManager.AddItem(Server, client.Character, true, gearupgradeID, 1 * 1); // Need AddItemResult param in here too because we use it on Response.
                updateCharacterItemNtc.UpdateItemList.AddRange(AddItemResult);                              // For now I just give the item the same as though it wasn't equipped.

                Logger.Debug("EQUIPPED");

                // TODO: Figure out how to exchange the equipment correctly.


            }
            else
            {
                RemoveItemResult = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, equipItemUID, 1);
                bool isBoxItem = RemoveItemResult.Any(result => result.ItemList.StorageType == StorageType.StorageBoxNormal ||
                                                                result.ItemList.StorageType == StorageType.StorageBoxExpansion);
                AddItemResult = _itemManager.AddItem(Server, client.Character, isBoxItem, gearupgradeID, 1 * 1);
                updateCharacterItemNtc.UpdateItemList.AddRange(RemoveItemResult);
                updateCharacterItemNtc.UpdateItemList.AddRange(AddItemResult);

                var addedStorageTypes = GetStorageTypes(AddItemResult);
                foreach (var storageType in addedStorageTypes)
                {
                    Logger.Debug($"Added item in storage type: {storageType}, and your bool is- {isBoxItem}");
                }
            };


            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
                Unk0 = charid,
                Unk1 = pawnid,
                Unk2 = equiptype, // type
                Unk3 = equipslot, // slot
            };
            CDataCurrentEquipInfo cei = new CDataCurrentEquipInfo()
            {
                ItemUID = equipItemUID,
                EquipSlot = EquipmentSlot
            };


            // Supplying the response packet with data
            var res = new S2CCraftStartEquipGradeUpRes()
            {
                GradeUpItemUID = AddItemResult[0].ItemList.ItemUId, // Setting this to equipItemUID makes the results info box be accurate, but setting it to this stops upgrading multiple pieces.
                GradeUpItemID = gearupgradeID, // This has to be the upgrade step ID.
                //GradeUpItemIDList = dummygradeuplist, // This list should start with the next ID.
                AddEquipPoint = addEquipPoint,              
                TotalEquipPoint = currentTotalEquipPoint,
                EquipGrade = nextGrade, // It expects a valid number or it won't show the result when you enhance, (presumably we give this value when filling the bar)
                Gold = goldRequired, // No noticable difference when supplying this info, but it wants it so whatever.
                IsGreatSuccess = dogreatsucess, // Just changes the banner from "Success" to "GreatSuccess" we'd have to augment the addEquipPoint value when this is true.
                CurrentEquip = cei, // Dummy current equip data, need to get the real slot/type at somepoint.               
                BeforeItemID = equipItemID, // I don't know why the response wants the "beforeid" its unclear what this means too? should it be 0 if step 1? hmm.
                Unk0 = true, // Unk0 being true says "Grade Up" when filling rather than "Grade Max", so we need to track when we hit max upgrade.
                Unk1 = dummydata // Since nothing appears to happen this is probably related to equipped gradeup gear.
            };
            client.Send(res);
            client.Send(updateCharacterItemNtc);
        }

        // Method to extract storage types from the item update result list
        private List<StorageType> GetStorageTypes(List<CDataItemUpdateResult> itemUpdateResults)
        {
            return itemUpdateResults.Select(result => result.ItemList.StorageType).ToList();
        }
    }
}