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

        private readonly ItemManager _itemManager;

        public CraftStartQualityUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
        }

        public override S2CCraftStartQualityUpRes Handle(GameClient client, C2SCraftStartQualityUpReq request)
        {  
            string equipItemUID = request.ItemUID;
            var equipItem = Server.Database.SelectStorageItemByUId(equipItemUID);
            Character character = client.Character;
            uint pawnid = request.CraftMainPawnID;
            bool IsGreatSuccess = Random.Shared.Next(5) == 0;
            string RefineMaterial = request.RefineUID;
            ushort AddStatusID = request.AddStatusID;
            byte RandomQuality = 0;
            int D100 =  Random.Shared.Next(100);
            List<CDataItemUpdateResult> updateResults;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();  

            CDataAddStatusParam AddStat = new CDataAddStatusParam()
            {
                IsAddStat1 = false,
                IsAddStat2 = false,
                AdditionalStatus1 = 0,
                AdditionalStatus2 = 0,
            };
            List<CDataAddStatusParam> AddStatList = new List<CDataAddStatusParam>()
            {
                AddStat
            };
            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
                EquipSlot = EquipmentSlot
            };
            // TODO: figuring out what this is
            // I've tried plugging Crest IDs & Equipment ID/RandomQuality n such, and just random numbers Unk0 - Unk4 just don't seem to change anything.
            CDataS2CCraftStartQualityUpResUnk0 dummydata = new CDataS2CCraftStartQualityUpResUnk0()
            {
                Unk0 = 0, // Potentially an ID?
                Unk1 = 0,
                Unk2 = 0, // Genuinely no idea what this could be for. 
                Unk3 = 0, // Potentially an ID for something?
                Unk4 = 0, // Potentially an ID for something too?
                IsGreatSuccess = IsGreatSuccess
            };

            // TODO: Revisit AdditionalStatus down the line. It appears it might be apart of a larger system involving craig? 
            // Definitely a potential huge rabbit hole that I think we should deal with in a different PR.

            //TODO: There are 3 tiers, and the lowest tier can't become +3, and the highest has better chance of +3, so we need to do a direct ID comparison,
            // So a total of 6 IDs? for armor and weapons. 3 each.
            if (!string.IsNullOrEmpty(RefineMaterial))
            {
                updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, RefineMaterial, 1);
                updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
            }

            var thresholds = new (int Threshold, int Quality)[]
            {
                (65, 2),
                (15, 1),
                (0, 0)
            };

            RandomQuality = (byte)thresholds.First(t => D100 >= t.Threshold).Quality;

            if (IsGreatSuccess)
            {
                RandomQuality = 3;
            }
            if (equipItem.PlusValue > RandomQuality)
            {
                RandomQuality = equipItem.PlusValue;
                // Wiki's say you can't lower quality.
            }

            // Updating the item.
            equipItem.ItemId = equipItem.ItemId;
            equipItem.PlusValue = RandomQuality;
            equipItem.AddStatusParamList = equipItem.AddStatusParamList;

            var (storageType, foundItem) = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
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
                    CurrentEquipInfo.EquipSlot.PawnId = pawnid;
                    characterCommon = character.Pawns.Where(x => x.PawnId == pawnid).SingleOrDefault();
                }
                else if(storageType == StorageType.CharacterEquipment)
                {
                    CurrentEquipInfo.EquipSlot.CharacterId = character.CharacterId;
                    characterCommon = character;
                }
                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, (byte)slotno, 0, 0));
                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    _itemManager.UpgradeStorageItem(
                        Server,
                        client,
                        character.CharacterId,
                        storageType,
                        equipItem,
                        (byte)slotno
                    );
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, (byte)slotno, 1, 1));
                    client.Send(updateCharacterItemNtc);
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                }
            }
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