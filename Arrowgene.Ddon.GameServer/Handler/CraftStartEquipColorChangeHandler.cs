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
    public class CraftStartEquipColorChangeHandler : GameRequestPacketHandler<C2SCraftStartEquipColorChangeReq, S2CCraftStartEquipColorChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipColorChangeHandler));
        private readonly ItemManager _itemmanager;
        public CraftStartEquipColorChangeHandler(DdonGameServer server) : base(server)
        {
            _itemmanager = server.ItemManager;
        }

        public override S2CCraftStartEquipColorChangeRes Handle(GameClient client, C2SCraftStartEquipColorChangeReq request)
        {
            Character character = client.Character;
            uint charid = client.Character.CharacterId;
            string equipItemUID = request.EquipItemUID;
            List<CDataCraftColorant> colorList = request.CraftColorantList;
            Item equipItem = Server.Database.SelectStorageItemByUId(equipItemUID);
            byte color = request.Color;
            List<CDataCraftColorant> colorlist = new List<CDataCraftColorant>(); // this is probably for consuming the dye
            uint craftpawnid = request.CraftMainPawnID;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
            };
            var colorantList = colorList[0];
            byte number = colorantList.ItemNum;
            equipItem.Color = color;

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
                else if(storageType == StorageType.CharacterEquipment)
                {
                    CurrentEquipInfo.EquipSlot.CharacterId = charid;
                    characterCommon = character;
                }

                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipColorChang;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, (byte)slotno, 0, 0));
                
                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    _itemmanager.UpgradeStorageItem(
                        Server,
                        client,
                        charid,
                        storageType,
                        equipItem,
                        (byte)slotno
                    );
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, (byte)slotno, 1, 1));
                    client.Send(updateCharacterItemNtc);
                }
            }
            else
            {
                Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
            }
            
            // TODO: Potentially the packets changed in S3.
            
            var res = new S2CCraftStartEquipColorChangeRes()
                {
                    ColorNo = color,
                    CurrentEquipInfo = CurrentEquipInfo
                };

            // TODO: Store saved pawn exp
            S2CCraftCraftExpUpNtc expNtc = new S2CCraftCraftExpUpNtc()
            {
                PawnId = request.CraftMainPawnID,
                AddExp = 10,
                ExtraBonusExp = 0,
                TotalExp = 10,
                CraftRankLimit = 0
            };
            client.Send(expNtc);
            
            return res;
        }
    }
}