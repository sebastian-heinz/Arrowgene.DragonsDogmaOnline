#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class ItemManager
    {
        private static readonly Dictionary<uint, (WalletType Type, uint Quantity)> ItemIdWalletTypeAndQuantity = new Dictionary<uint, (WalletType Type, uint Amount)>() { 
            {7789, (WalletType.Gold, 1)},
            {7790, (WalletType.Gold, 10)},
            {7791, (WalletType.Gold, 100)},
            {7792, (WalletType.RiftPoints,1)},
            {7793, (WalletType.RiftPoints,10)},
            {7794, (WalletType.RiftPoints,100)},
            {7795, (WalletType.BloodOrbs,1)}, // Doesn't show up 
            {7796, (WalletType.BloodOrbs,10)}, // Doesn't show up
            {7797, (WalletType.BloodOrbs,100)}, // Doesn't show up
            {18742, (WalletType.HighOrbs,1)},
            {18743, (WalletType.HighOrbs,10)},
            {18744, (WalletType.HighOrbs,100)},
            {18828,(WalletType.Gold,7500)},
            {18829,(WalletType.RiftPoints,1250)},
            {18830,(WalletType.BloodOrbs,750)},
            {19508,(WalletType.Gold,1000)},
            {19509,(WalletType.Gold,10000)},
            {19510,(WalletType.RiftPoints,1000)},
            {19511,(WalletType.BloodOrbs,1000)}
            // TODO: Find all items that add wallet points
        };

        // [[item]]
        // id = 16822 (Adds 100 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'
        // [[item]]
        // id = 16831 (Adds 10000 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'
        // [[item]]
        // id = 18831 (Adds 63000 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'

        // [[item]]
        // id = 18832 (Adds 18 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25651 (Adds 1 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25652 (Adds 10 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25653 (Adds 100 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'

        public void GatherItem(DdonServer<GameClient> server, Character character, S2CItemUpdateCharacterItemNtc ntc, InstancedGatheringItem gatheringItem, uint pickedGatherItems)
        {
            if(ItemIdWalletTypeAndQuantity.ContainsKey(gatheringItem.ItemId)) {
                var walletTypeAndQuantity = ItemIdWalletTypeAndQuantity[gatheringItem.ItemId];
                uint totalQuantityToAdd = walletTypeAndQuantity.Quantity * gatheringItem.ItemNum;
                
                CDataWalletPoint characterWalletPoint = character.WalletPointList.Where(wp => wp.Type == walletTypeAndQuantity.Type).First();
                characterWalletPoint.Value += totalQuantityToAdd; // TODO: Cap to maximum for that wallet
                server.Database.UpdateWalletPoint(character.CharacterId, characterWalletPoint);

                CDataUpdateWalletPoint walletUpdate = new CDataUpdateWalletPoint();
                walletUpdate.Type = walletTypeAndQuantity.Type;
                walletUpdate.AddPoint = (int) totalQuantityToAdd;
                walletUpdate.Value = characterWalletPoint.Value;
                ntc.UpdateWalletList.Add(walletUpdate);
                
                gatheringItem.ItemNum -= pickedGatherItems;
            } else {
                CDataItemUpdateResult? result = AddItem(server, character, true, gatheringItem.ItemId, pickedGatherItems);
                ntc.UpdateItemList.Add(result);
                gatheringItem.ItemNum -= (uint) result.UpdateItemNum;
            }
        }

        public List<CDataItemUpdateResult> ConsumeItemByUIdFromMultipleStorages(DdonServer<GameClient> server, Character character, List<StorageType> fromStorageTypes, string itemUId, uint consumeNum)
        {
            int remainingItems = (int) consumeNum;
            List<CDataItemUpdateResult> results = new List<CDataItemUpdateResult>();
            foreach (StorageType storageType in fromStorageTypes)
            {
                CDataItemUpdateResult? result = ConsumeItemByUId(server, character, storageType, itemUId, (uint) remainingItems);
                if (result != null)
                {
                    results.Add(result);
                    remainingItems += result.UpdateItemNum;
                    if (remainingItems == 0)
                    {
                        return results;
                    }
                }
            }

            // TODO: Rollback transaction
            throw new NotEnoughItemsException(itemUId, consumeNum, remainingItems);
        }

        public CDataItemUpdateResult? ConsumeItemByUId(DdonServer<GameClient> server, Character character, StorageType fromStorageType, string itemUId, uint consumeNum)
        {
            var foundItem = character.Storage.getStorage(fromStorageType).findItemByUId(itemUId);
            if(foundItem == null)
            {
                return null;
            } else {
                (ushort slotNo, Item item, uint itemNum) = foundItem;
                return ConsumeItem(server, character, fromStorageType, slotNo, item, itemNum, consumeNum);
            }
        }
        public CDataItemUpdateResult? ConsumeItemInSlot(DdonServer<GameClient> server, Character character, StorageType fromStorageType, ushort slotNo, uint consumeNum)
        {
            var foundItem = character.Storage.getStorageItem(fromStorageType, slotNo);
            if(foundItem == null)
            {
                return null;
            } else {
                (Item item, uint itemNum) = foundItem;
                return ConsumeItem(server, character, fromStorageType, slotNo, item, itemNum, consumeNum);
            }
        }
        private CDataItemUpdateResult ConsumeItem(DdonServer<GameClient> server, Character character, StorageType fromStorageType, ushort slotNo, Item item, uint itemNum, uint consuneNum)
        {
            uint finalItemNum = (uint) Math.Max(0, (int)itemNum - (int)consuneNum);
            int finalConsumeNum = (int)itemNum - (int)finalItemNum;

            CDataItemUpdateResult ntcData = new CDataItemUpdateResult();
            ntcData.ItemList.ItemUId = item.UId;
            ntcData.ItemList.ItemId = item.ItemId;
            ntcData.ItemList.ItemNum = finalItemNum;
            ntcData.ItemList.Unk3 = item.Unk3;
            ntcData.ItemList.StorageType = fromStorageType;
            ntcData.ItemList.SlotNo = slotNo;
            ntcData.ItemList.Color = item.Color;
            ntcData.ItemList.PlusValue = item.PlusValue;
            ntcData.ItemList.Bind = false;
            ntcData.ItemList.EquipPoint = 0;
            ntcData.ItemList.EquipCharacterID = 0;
            ntcData.ItemList.EquipPawnID = 0;
            ntcData.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
            ntcData.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
            ntcData.ItemList.EquipElementParamList = item.EquipElementParamList;
            ntcData.UpdateItemNum = -finalConsumeNum;

            if(finalItemNum == 0)
            {
                // Delete item when ItemNum reaches 0 to free up the slot
                character.Storage.setStorageItem(null, 0, fromStorageType, slotNo);
                server.Database.DeleteStorageItem(character.CharacterId, fromStorageType, slotNo);
            }
            else
            {
                character.Storage.setStorageItem(item, finalItemNum, fromStorageType, slotNo);
                server.Database.ReplaceStorageItem(character.CharacterId, fromStorageType, slotNo, item.UId, finalItemNum);
            }

            return ntcData;
        }

        public CDataItemUpdateResult AddItem(DdonServer<GameClient> server, Character character, bool itemBag, uint itemId, uint num)
        {
            if(itemBag)
            {
                ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, itemId);
                return AddItem(server.Database, character, clientItemInfo.StorageType, itemId, num, clientItemInfo.StackLimit);
            }
            else
            {
                // TODO: Does the Storage Box have an item cap?
                return AddItem(server.Database, character, StorageType.StorageBoxNormal, itemId, num);
            }

        }
        private CDataItemUpdateResult AddItem(IDatabase database, Character character, StorageType destinationStorageType, uint itemId, uint num, uint stackLimit = UInt32.MaxValue)
        {
            var tuple = character.Storage.getStorage(destinationStorageType).Items
                .Select((item, index) => new {item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item?.Item1.ItemId == itemId)
                .FirstOrDefault();

            Item? item = tuple?.item?.Item1;
            ushort slot = tuple?.slot ?? 0;
            uint oldItemNum = tuple?.item?.Item2 ?? 0;
            uint newItemNum = Math.Min(stackLimit, oldItemNum + num);
            uint addedItems = newItemNum - oldItemNum;
            
            if (item == null)
            {
                item = new Item() {
                    ItemId = itemId,
                    Unk3 = 0,
                    Color = 0,
                    PlusValue = 0,
                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                    ArmorCrestDataList = new List<CDataArmorCrestData>(),
                    EquipElementParamList = new List<CDataEquipElementParam>()
                };
                database.InsertItem(item);
                slot = character.Storage.addStorageItem(item, newItemNum, destinationStorageType);
            } else
            {
                character.Storage.setStorageItem(item, newItemNum, destinationStorageType, slot);
            }

            database.ReplaceStorageItem(character.CharacterId, destinationStorageType, slot, item.UId, newItemNum);

            CDataItemUpdateResult result = new CDataItemUpdateResult();
            result.ItemList.ItemUId = item.UId;
            result.ItemList.ItemId = item.ItemId;
            result.ItemList.ItemNum = newItemNum;
            result.ItemList.Unk3 = item.Unk3;
            result.ItemList.StorageType = destinationStorageType;
            result.ItemList.SlotNo = slot;
            result.ItemList.Color = item.Color; // ?
            result.ItemList.PlusValue = item.PlusValue; // ?
            result.ItemList.Bind = false;
            result.ItemList.EquipPoint = 0;
            result.ItemList.EquipCharacterID = 0;
            result.ItemList.EquipPawnID = 0;
            result.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
            result.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
            result.ItemList.EquipElementParamList = item.EquipElementParamList;
            result.UpdateItemNum = (int) addedItems;
            return result;
        }
    }

    [Serializable]
    internal class NotEnoughItemsException : Exception
    {
        private string itemUId;
        private uint consumeNum;
        private int remainingItems;

        public NotEnoughItemsException(string itemUId, uint consumeNum, int remainingItems) : base($"Required {consumeNum} items of UID {itemUId}, missing {remainingItems} items")
        {
            this.itemUId = itemUId;
            this.consumeNum = consumeNum;
            this.remainingItems = remainingItems;
        }
    }
}