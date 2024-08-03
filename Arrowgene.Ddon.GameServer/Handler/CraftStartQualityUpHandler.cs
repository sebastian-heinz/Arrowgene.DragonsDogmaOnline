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
using System.Data.SqlTypes;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartQualityUpHandler : GameRequestPacketHandler<C2SCraftStartQualityUpReq, S2CCraftStartQualityUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartQualityUpHandler));
        private static readonly List<StorageType> StorageEquipNBox = new List<StorageType> {
            StorageType.ItemBagEquipment, StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion,
            StorageType.StorageChest, StorageType.CharacterEquipment, StorageType.PawnEquipment
        };

        private readonly ItemManager _itemManager;
        private readonly EquipManager _equipManager;
        private readonly Random _random;

        public CraftStartQualityUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
            _equipManager = Server.EquipManager;
            _random = Random.Shared;
        }

        public override S2CCraftStartQualityUpRes Handle(GameClient client, C2SCraftStartQualityUpReq request)
        {  
            string equipItemUID = request.ItemUID;
            var equipItem = Server.Database.SelectStorageItemByUId(equipItemUID);
            Character character = client.Character;
            uint charid = client.Character.CharacterId;
            uint pawnid = request.CraftMainPawnID;
            bool dogreatsucess = _random.Next(5) == 0; // 1 in 5 chance to be true, someone said it was 20%.
            // byte currentPlusValue = equipItem.PlusValue;
            // bool retainPlusValue = false;
            // bool updatingAddStatus = false;
            string RefineMaterial = request.RefineUID;
            ushort AddStatusID = request.AddStatusID;
            byte RandomQuality = 0;
            int D100 =  _random.Next(100);
            List<CDataItemUpdateResult> updateResults;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();  

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

            
            var thresholds = new (int Threshold, int Quality)[]
            {
                (65, 2),
                (15, 1),
                (0, 0)
            };
            // TODO: Supposedly the base Refinement material cannot hit 3, only 1, unless it greatsuccess in which it can hit 2.
            // 3 appears to be exclusive to the better refinement material?

            RandomQuality = (byte)thresholds.First(t => D100 >= t.Threshold).Quality;

            if (dogreatsucess)
            {
                RandomQuality = 3;
            }
            
            // TODO: Revisit AdditionalStatus down the line. It appears it might be apart of a larger system involving craig? 
            // Definitely a potential huge rabbit hole that I think we should deal with in a different PR.

            List<CDataAddStatusData> AddStatList = new List<CDataAddStatusData>()
            {
                AddStat
            };

            if (equipItem.PlusValue > RandomQuality)
            {
                RandomQuality = equipItem.PlusValue;
                // Wiki's say you can't lower quality.
            }

            // Updating the item.
            equipItem.ItemId = equipItem.ItemId;
            equipItem.Unk3 = equipItem.Unk3;
            equipItem.Color = equipItem.Color;
            equipItem.PlusValue = RandomQuality;
            equipItem.WeaponCrestDataList = equipItem.WeaponCrestDataList;
            equipItem.AddStatusData = equipItem.AddStatusData;
            equipItem.EquipElementParamList = equipItem.EquipElementParamList;

            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
                CharacterId = 0,
                PawnId = 0,
                EquipType = 0,
                EquipSlotNo = 0,
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
                EquipSlot = EquipmentSlot
            };

            
            Logger.Debug($"Attempting to find {equipItemUID}");
            var (storageType, foundItem) = character.Storage.FindItemByUIdInStorage(StorageEquipNBox, equipItemUID);

            if (foundItem != null)
            {
                Logger.Debug($"Found {equipItemUID} inside {storageType}");
                // Extract slotno, item, itemnum from the foundItem tuple
                var (slotno, item, itemnum) = foundItem;


                CharacterCommon characterCommon = null;
                if (storageType == StorageType.CharacterEquipment || storageType == StorageType.PawnEquipment)
                {
                    CurrentEquipInfo.EquipSlot.EquipSlotNo = EquipManager.DetermineEquipSlot(slotno);
                    CurrentEquipInfo.EquipSlot.EquipType = EquipManager.GetEquipTypeFromSlotNo(slotno);
                }
                if (storageType == StorageType.PawnEquipment)
                {
                    CurrentEquipInfo.EquipSlot.PawnId = pawnid;
                    characterCommon = character.Pawns.Where(x => x.PawnId == pawnid).SingleOrDefault();
                }
                else if(storageType == StorageType.CharacterEquipment)
                {
                    CurrentEquipInfo.EquipSlot.CharacterId = charid;
                    characterCommon = character;
                }


                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, (byte)slotno, 0, 0));
                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    updateResults = _itemManager.UpgradeStorageItem(
                        Server,
                        client,
                        character,
                        charid,
                        storageType,
                        equipItem,
                        (byte)slotno
                    );
                    Logger.Debug($"Your Slot is: {slotno}, in {storageType} for UID {equipItem.UId}.");
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, (byte)slotno, 1, 1));

                    client.Send(updateCharacterItemNtc);
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                }
            }
            

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
    }
}