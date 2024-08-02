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
    public class CraftStartQualityUpHandler : GameRequestPacketHandler<C2SCraftStartQualityUpReq, S2CCraftStartQualityUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartQualityUpHandler));
        private static readonly List<StorageType> StorageEquipNBox = new List<StorageType> {
            StorageType.ItemBagEquipment, StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest, StorageType.CharacterEquipment
        };

        private readonly ItemManager _itemManager;
        private readonly Random _random;

        public CraftStartQualityUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
            _random = Random.Shared;
        }

        public override S2CCraftStartQualityUpRes Handle(GameClient client, C2SCraftStartQualityUpReq request)
        {

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();    
            updateCharacterItemNtc.UpdateType = 0;
            string equipItemUID = request.ItemUID;
            var equipItem = Server.Database.SelectStorageItemByUId(equipItemUID);
            uint equipItemID = equipItem.ItemId;
            Character common = client.Character;
            ushort equipslot = 0;
            byte equiptype = 0;
            uint charid = client.Character.CharacterId;
            uint pawnid = request.CraftMainPawnID;
            byte currentPlusValue = equipItem.PlusValue;
            bool dogreatsucess = _random.Next(5) == 0; // 1 in 5 chance to be true, someone said it was 20%.
            bool retainPlusValue = false;
            bool updatingAddStatus = false;
            string RefineMaterial = request.RefineUID;
            ushort AddStatusID = request.AddStatusID;
            byte RandomQuality = 0;
            int D100 =  _random.Next(100);
            List<CDataItemUpdateResult> updateResults;
            CDataAddStatusData AddStat = new CDataAddStatusData()
            {
                IsAddStat1 = 0,
                IsAddStat2 = 0,
                AdditionalStatus1 = 0,
                AdditionalStatus2 = 0,
            };


            //TODO: There are 3 tiers, and the lowest tier can't become +3, and the highest has better chance of +3, so we need to do a direct ID comparison,
            // So a total of 6 IDs? for armor and weapons. 3 each.
            if (!string.IsNullOrEmpty(RefineMaterial))
            {
                foreach (var craftMaterial in request.CraftMaterialList)
                {
                        updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, RefineMaterial, 1);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
            }
            else
            {
                retainPlusValue = true; // This exists because you can change the additionalstatus whenever you want, even with one applied.
                                        // However PlusValue (Quality) shouldn't get re-rolled if you're only changing additionalstatus.
            }

            
            var thresholds = new (int Threshold, int Quality)[]
            {
                (75, 2),
                (25, 1),
                (0, 0)  // This should always be the last one to catch all remaining cases
            };

            RandomQuality = (byte)thresholds.First(t => D100 >= t.Threshold).Quality;

            if (dogreatsucess)
            {
                RandomQuality = 3;
            }
            
            if (AddStatusID > 0 && updatingAddStatus == false)
            {
                
                bool success = Server.Database.InsertIfNotExistsAddStatus(equipItemUID, charid, 1, 0, AddStatusID, 0);
                AddStat = new CDataAddStatusData()
                {
                    IsAddStat1 = 1,
                    IsAddStat2 = 0,
                    AdditionalStatus1 = AddStatusID,
                    AdditionalStatus2 = 0,
                };

                if (success)
                    {
                        Console.WriteLine("Additional status added successfully.");
                    }
                else
                    {
                        success = Server.Database.UpdateAddStatus(equipItemUID, charid, 1, 0, AddStatusID, 0);
                        AddStat = new CDataAddStatusData()
                        {
                            IsAddStat1 = 1,
                            IsAddStat2 = 0,
                            AdditionalStatus1 = AddStatusID,
                            AdditionalStatus2 = 0,
                        };
                        if (success)
                        {
                            Console.WriteLine("Additional status Updated successfully.");
                        }
                    };
            };



            List<CDataAddStatusData> AddStatList = new List<CDataAddStatusData>()
            {
                AddStat
            };


            if(retainPlusValue)
            {
                RandomQuality = currentPlusValue;
            }


            // Updating the item.
            equipItem.ItemId = equipItemID;
            equipItem.Unk3 = equipItem.Unk3;
            equipItem.Color = equipItem.Color;
            equipItem.PlusValue = RandomQuality;
            equipItem.WeaponCrestDataList = equipItem.WeaponCrestDataList;
            equipItem.AddStatusData = AddStatList;
            equipItem.EquipElementParamList = equipItem.EquipElementParamList;

            
            Logger.Debug($"Attempting to find {equipItemUID}");
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
                            equipslot = equipInfo.EquipCategory;
                            equiptype = (byte)equipInfo.EquipType;

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
                        Logger.Debug($"Bruh this found an item in {storageType}, not cool.");
                        break;
                }


                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(common, equipItem, storageType, (byte)slotno, 0, 0));
                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    updateResults = _itemManager.UpdateStorageItem(
                        Server,
                        client,
                        common,
                        charid,
                        storageType,
                        equipItem,
                        (byte)slotno
                    );
                    updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                    Logger.Debug($"Your Slot is: {slotno}, in {storageType} hopefully thats right?");
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(common, equipItem, storageType, (byte)slotno, 1, 1));
                    client.Send(updateCharacterItemNtc);
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                }
            

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

            // TODO: figuring out what this is
            // I've tried plugging Crest IDs & Equipment ID/RandomQuality n such, and just random numbers Unk0 - Unk4 just don't seem to change anything.
            // I think this must be related to Dragon Augment?, since plugging in a bunch of data has 0 noticable changes.
            CDataS2CCraftStartQualityUpResUnk0 dummydata = new CDataS2CCraftStartQualityUpResUnk0()
            {
                Unk0 = 0, // Potentially an ID?
                Unk1 = 0,
                Unk2 = 0, // Genuinely no idea what this could be for. 
                Unk3 = 0, // Potentially an ID for something?
                Unk4 = 0, // Potentially an ID for something too?
                IsGreatSuccess = dogreatsucess
            };

            var res = new S2CCraftStartQualityUpRes()
            {
                Unk0 = dummydata,
                AddStatusDataList = AddStatList,
                CurrentEquip = CurrentEquipInfo
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