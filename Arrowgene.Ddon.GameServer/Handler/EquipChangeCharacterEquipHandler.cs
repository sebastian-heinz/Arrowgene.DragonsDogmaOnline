using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterEquipHandler : GameStructurePacketHandler<C2SEquipChangeCharacterEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterEquipHandler));

        public EquipChangeCharacterEquipHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterEquipReq> packet)
        {
            EquipChangeCharacterEquipHandler.HandleChangeCharacterEquipList(Server, client, packet.Structure.ChangeCharacterEquipList, 0x24, StorageType.ItemBagEquipment, () => {
                client.Send(new S2CEquipChangeCharacterEquipRes()
                {
                    CharacterEquipList = packet.Structure.ChangeCharacterEquipList
                });
            });
        }

        public static void HandleChangeCharacterEquipList(DdonGameServer server, GameClient client, List<CDataCharacterEquipInfo> changeCharacterEquipList, ushort updateType, StorageType storageType, Action sendResponse)
        {
            // This'll contain all actions to be run after processing all CDataCharacterEquipInfo in the request packet
            List<Action> deferredActions = new List<Action>();

            S2CItemUpdateCharacterItemNtc equipUpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            equipUpdateCharacterItemNtc.UpdateType = updateType;

            S2CItemUpdateCharacterItemNtc unequipUpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            unequipUpdateCharacterItemNtc.UpdateType = updateType;

            foreach (CDataCharacterEquipInfo changeCharacterEquipInfo in changeCharacterEquipList)
            {
                string itemUId = changeCharacterEquipInfo.EquipItemUId;
                EquipType equipType = (EquipType) changeCharacterEquipInfo.EquipType;
                byte equipSlot = changeCharacterEquipInfo.EquipCategory;

                if(itemUId.Length == 0)
                {
                    EquipChangeCharacterEquipHandler.UnequipItem(server, client, unequipUpdateCharacterItemNtc, equipType, equipSlot, storageType);
                }
                else
                {
                    EquipChangeCharacterEquipHandler.EquipItem(server, client, equipUpdateCharacterItemNtc, unequipUpdateCharacterItemNtc, equipType, equipSlot, storageType, itemUId);
                }
            }

            sendResponse.Invoke();

            // Notify other players
            S2CEquipChangeCharacterEquipNtc changeCharacterEquipNtc = new S2CEquipChangeCharacterEquipNtc()
            {
                CharacterId = client.Character.Id,
                EquipItemList = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Performance),
                VisualEquipItemList = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Visual)
                // TODO: Unk0
            };

            foreach (Client otherClient in server.ClientLookup.GetAll())
            {
                otherClient.Send(changeCharacterEquipNtc);
            }
            
            // This is the only way i've found for it to behave properly.
            // If i try to send equipping data (aka, removing from the bag the equipped item)
            //  without sending the first NTC even if it's empty AND SLEEPING (IMPORTANT), the item doesn't show up as equipped.
            // And if I don't send the equipping data, the item is duplicated, it appears as equipped AND still in the item bag
            // I have NO IDEA why you have to do it this way
            client.Send(unequipUpdateCharacterItemNtc);
            System.Threading.Thread.Sleep(100);
            client.Send(equipUpdateCharacterItemNtc);
        }

        private static void UnequipItem(DdonGameServer server, GameClient client, S2CItemUpdateCharacterItemNtc unequipUpdateCharacterItemNtc, EquipType equipType, byte equipSlot, StorageType storageType)
        {
            // Find in equipment the item to unequip
            Item item = client.Character.Equipment.getEquipItem(client.Character.Job, equipType, equipSlot);

            client.Character.Equipment.setEquipItem(null, client.Character.Job, equipType, equipSlot);
            server.Database.DeleteEquipItem(client.Character.Id, client.Character.Job, equipType, equipSlot, item.UId);
            
            ushort dstSlotNo = client.Character.Storage.addStorageItem(item, 1, storageType);
            server.Database.InsertStorageItem(client.Character.Id, storageType, dstSlotNo, item.UId, 1);

            unequipUpdateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                UpdateItemNum = 0,
                ItemList = new CDataItemList() {
                    ItemUId = item.UId,
                    ItemId = item.ItemId,
                    ItemNum = 1,
                    Unk3 = item.Unk3,
                    StorageType = storageType,
                    SlotNo = dstSlotNo,
                    Color = item.Color,
                    PlusValue = item.PlusValue,
                    Bind = false,
                    EquipPoint = 0,
                    EquipCharacterID = 0,
                    EquipPawnID = 0,
                    WeaponCrestDataList = item.WeaponCrestDataList,
                    ArmorCrestDataList = item.ArmorCrestDataList,
                    EquipElementParamList = item.EquipElementParamList
                }
            });
        }

        private static void EquipItem(DdonGameServer server, GameClient client, S2CItemUpdateCharacterItemNtc equipUpdateCharacterItemNtc, S2CItemUpdateCharacterItemNtc unequipUpdateCharacterItemNtc, EquipType equipType, byte equipSlot, StorageType storageType, string itemUId)
        {
            // Find in the bag the item to equip
            var tuple = client.Character.Storage.getStorage(storageType).Items
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item?.Item1.UId == itemUId)
                .First();
            Item item = tuple.item.Item1;
            uint itemNum = tuple.item.Item2;
            ushort srcSlotNo = tuple.slot;

            Item itemInSlot = client.Character.Equipment.getEquipItem(client.Character.Job, (EquipType) equipType, equipSlot);
            if(itemInSlot != null)
            {
                // When equipping over an already equipped slot, unequip item first
                EquipChangeCharacterEquipHandler.UnequipItem(server, client, unequipUpdateCharacterItemNtc, equipType, equipSlot, storageType);
            }

            client.Character.Equipment.setEquipItem(item, client.Character.Job, (EquipType) equipType, equipSlot);
            server.Database.InsertEquipItem(client.Character.Id, client.Character.Job, equipType, equipSlot, item.UId);

            // Find slot from which the item will be taken
            client.Character.Storage.setStorageItem(null, 0, storageType, tuple.slot);
            server.Database.DeleteStorageItem(client.Character.Id, storageType, tuple.slot);

            equipUpdateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                UpdateItemNum = 0, // TODO: ?
                ItemList = new CDataItemList() {
                    ItemUId = item.UId,
                    ItemId = item.ItemId,
                    ItemNum = 0,
                    Unk3 = item.Unk3,
                    StorageType = storageType,
                    SlotNo = srcSlotNo,
                    Color = item.Color,
                    PlusValue = item.PlusValue,
                    Bind = true,
                    EquipPoint = 0,
                    EquipCharacterID = client.Character.Id,
                    EquipPawnID = 0,
                    WeaponCrestDataList = item.WeaponCrestDataList,
                    ArmorCrestDataList = item.ArmorCrestDataList,
                    EquipElementParamList = item.EquipElementParamList
                }
            });
        }
    }

}