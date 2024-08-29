#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Chat.Command.Commands;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftEquipEnhancedEnhanceItemHandler : GameRequestPacketHandler<C2SCraftEquipEnhancedEnhanceItemReq, S2CCraftEquipEnhancedEnhanceItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftEquipEnhancedEnhanceItemHandler));


        public CraftEquipEnhancedEnhanceItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftEquipEnhancedEnhanceItemRes Handle(GameClient client, C2SCraftEquipEnhancedEnhanceItemReq request)
        {
            Character character = client.Character;
            uint charid = client.Character.CharacterId;
            string equipItemUID = request.Unk1;
            string limitBreakMaterialUID = request.Unk2;
            ushort limitBreakID = request.Unk0;
            var ramItem = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            var equipItem = ramItem.Item2.Item2;
            ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItem.ItemId);

            CDataCraftEquipEnhancedEnhanceItemReqUnk0 testingunk0 = new CDataCraftEquipEnhancedEnhanceItemReqUnk0();
            CDataCraftEquipEnhancedEnhanceItemReqUnk1 testingunk1 = new CDataCraftEquipEnhancedEnhanceItemReqUnk1();


            List<CDataItemUpdateResult> updateResults;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

            CDataAddStatusParam AddStat1 = new CDataAddStatusParam()
            {
                IsAddStat1 = 1,
                IsAddStat2 = true,
                AdditionalStatus1 = limitBreakID,
                AdditionalStatus2 = 27,
            };
            CDataAddStatusParam AddStat2 = new CDataAddStatusParam() // 6 star test
            {
                IsAddStat1 = 2,
                IsAddStat2 = false,
                AdditionalStatus1 = 0,
                AdditionalStatus2 = 0,
            };
            List<CDataAddStatusParam> AddStatList = new List<CDataAddStatusParam>()
            {
                new CDataAddStatusParam(),
            };

            CDataCurrentEquipInfo currentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
            };
            AddStatList.Add(AddStat1);
            AddStatList.Add(AddStat2);

            equipItem.AddStatusParamList = AddStatList;

            var (storageType, foundItem) = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            if (foundItem != null)
            {
                var (slotno, item, itemnum) = foundItem;
                CharacterCommon characterCommon = null;
                if (storageType == StorageType.CharacterEquipment || storageType == StorageType.PawnEquipment)
                {
                    currentEquipInfo.EquipSlot.EquipSlotNo = EquipManager.DetermineEquipSlot(slotno);
                    currentEquipInfo.EquipSlot.EquipType = EquipManager.GetEquipTypeFromSlotNo(slotno);
                }

                if (storageType == StorageType.PawnEquipment)
                {
                    uint pawnId = Storages.DeterminePawnId(client.Character, storageType, slotno);
                    currentEquipInfo.EquipSlot.PawnId = pawnId;
                    characterCommon = client.Character.Pawns.SingleOrDefault(x => x.PawnId == pawnId);
                }
                else if (storageType == StorageType.CharacterEquipment)
                {
                    currentEquipInfo.EquipSlot.CharacterId = character.CharacterId;
                    characterCommon = character;
                }

                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 0, 0));

                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    Server.ItemManager.UpgradeStorageItem(Server, client, character.CharacterId, storageType, equipItem, slotno);
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 1, 1));
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
                }
            }

            var res = new S2CCraftEquipEnhancedEnhanceItemRes()
            {
                CurrentEquipInfo = new CDataCurrentEquipInfo()
            };
            client.Send(updateCharacterItemNtc);
            return res;
        }
    }
}
