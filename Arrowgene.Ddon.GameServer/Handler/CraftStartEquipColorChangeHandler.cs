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
            Item equipItem = Server.Database.SelectStorageItemByUId(equipItemUID);
            byte color = request.Color;
            List<CDataCraftColorant> colorlist = new List<CDataCraftColorant>(); // this is probably for consuming the dye
            uint pawnid = request.CraftMainPawnID;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
                EquipSlot = EquipmentSlot
            };

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
            
            // TODO: Potentially the packets changed in S3.
            
            var res = new S2CCraftStartEquipColorChangeRes()
                {
                    ColorNo = color,
                    CurrentEquipInfo = CurrentEquipInfo
                };
            return res;
        }
    }
}