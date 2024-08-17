#nullable enable
using System;
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
    public class CraftStartQualityUpHandler : GameRequestPacketHandler<C2SCraftStartQualityUpReq, S2CCraftStartQualityUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartQualityUpHandler));


        public CraftStartQualityUpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftStartQualityUpRes Handle(GameClient client, C2SCraftStartQualityUpReq request)
        {
            string equipItemUID = request.ItemUID;
            Character character = client.Character;
            var ramItem = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            Item equipItem = ramItem.Item2.Item2;
            uint craftpawnid = request.CraftMainPawnID;
            string RefineMaterialUID = request.RefineUID;
            ushort AddStatusID = request.AddStatusID;
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
                new CDataAddStatusParam(),
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
            };

            // TODO: Revisit AdditionalStatus down the line. It appears it might be apart of a larger system involving craig? 
            // Definitely a potential huge rabbit hole that I think we should deal with in a different PR.

            // Lead pawn is always owned by player.
            Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);
            List<Pawn> pawns = new List<Pawn> { leadPawn };
            pawns.AddRange(request.CraftSupportPawnIDList.Select(p => Server.CraftManager.FindPawn(client, p.PawnId, true)));
            List<uint> consumableQuantityLevels = pawns.Select(CraftManager.GetPawnConsumableQuantityLevel).ToList();

            uint plusValue = 0;
            bool isGreatSuccessEquipmentQuality = false;
            if (!string.IsNullOrEmpty(RefineMaterialUID))
            {
                Item refineMaterialItem = Server.Database.SelectStorageItemByUId(RefineMaterialUID);
                CraftCalculationResult craftCalculationResult = CraftManager.CalculateEquipmentQuality(refineMaterialItem, consumableQuantityLevels);
                plusValue = craftCalculationResult.CalculatedValue;
                isGreatSuccessEquipmentQuality = craftCalculationResult.IsGreatSuccess;

                try
                {
                    updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, RefineMaterialUID, 1);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
                catch (NotEnoughItemsException)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has Occurred.");
                }
            }

            // TODO: figuring out what this is
            // I've tried plugging Crest IDs & Equipment ID/RandomQuality n such, and just random numbers Unk0 - Unk4 just don't seem to change anything.
            CDataS2CCraftStartQualityUpResUnk0 dummydata = new CDataS2CCraftStartQualityUpResUnk0()
            {
                IsGreatSuccess = isGreatSuccessEquipmentQuality
            };

            // Updating the item.
            equipItem.ItemId = equipItem.ItemId;
            equipItem.PlusValue = (byte)plusValue;
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
                    uint pawnId = Storages.DeterminePawnId(client.Character, storageType, slotno);
                    CurrentEquipInfo.EquipSlot.PawnId = pawnId;
                    characterCommon = client.Character.Pawns.SingleOrDefault(x => x.PawnId == pawnId);
                }
                else if (storageType == StorageType.CharacterEquipment)
                {
                    CurrentEquipInfo.EquipSlot.CharacterId = character.CharacterId;
                    characterCommon = character;
                }

                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 0, 0));

                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    Server.ItemManager.UpgradeStorageItem(Server, client, character.CharacterId, storageType, equipItem, slotno);
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 1, 1));
                    client.Send(updateCharacterItemNtc);
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
                }
            }

            var res = new S2CCraftStartQualityUpRes()
            {
                Unk0 = dummydata,
                AddStatusDataList = AddStatList,
                CurrentEquip = CurrentEquipInfo
            };

            if (CraftManager.CanPawnExpUp(leadPawn))
            {
                // TODO: Find exp for item recipe
                CraftManager.HandlePawnExpUpNtc(client, leadPawn, 10, 0);
                if (CraftManager.CanPawnRankUp(leadPawn))
                {
                    CraftManager.HandlePawnRankUpNtc(client, leadPawn);
                }
                Server.Database.UpdatePawnBaseInfo(leadPawn);
            }

            return res;
        }
    }
}
