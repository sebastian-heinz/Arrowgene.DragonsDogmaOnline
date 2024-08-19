#nullable enable
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class ItemManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemManager));

        private static readonly uint STACK_BOX_MAX = 999;

        public static readonly List<StorageType> ItemBagStorageTypes = new List<StorageType> { 
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.KeyItems 
        };
        public static readonly List<StorageType> BoxStorageTypes = new List<StorageType> { 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, 
            StorageType.StorageChestDrawer1, StorageType.StorageChestDrawer2, StorageType.StorageChestDrawer3 
        };
        public static readonly List<StorageType> BothStorageTypes = ItemBagStorageTypes.Concat(BoxStorageTypes).ToList();
        public static readonly List<StorageType> EquipmentStorages = new List<StorageType> { 
            StorageType.CharacterEquipment, StorageType.PawnEquipment, 
            StorageType.ItemBagEquipment, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, 
            StorageType.StorageChestDrawer1, StorageType.StorageChestDrawer2, StorageType.StorageChestDrawer3 
        };

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
            {19511,(WalletType.BloodOrbs,1000)},
            // TODO: Requires special item notice type 47, could be offered in adventure pass shop
            {11262,(WalletType.ResetCraftSkills,1)}
            // TODO: Find all items that add wallet points
        };

        public bool IsItemWalletPoint(uint itemId)
        {
            return ItemIdWalletTypeAndQuantity.ContainsKey(itemId);
        }

        public (WalletType walletType, uint itemId) ItemToWalletPoint(uint itemId)
        {
            if (!IsItemWalletPoint(itemId))
            {
                return (WalletType.None, 0);
            }
            return ItemIdWalletTypeAndQuantity[itemId];
        }

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

        public void GatherItem(DdonServer<GameClient> server, Character character, S2CItemUpdateCharacterItemNtc ntc, InstancedGatheringItem gatheringItem, uint pickedGatherItems, DbConnection? connectionIn = null)
        {
            if(ItemIdWalletTypeAndQuantity.ContainsKey(gatheringItem.ItemId)) {
                var walletTypeAndQuantity = ItemIdWalletTypeAndQuantity[gatheringItem.ItemId];
                uint totalQuantityToAdd = walletTypeAndQuantity.Quantity * gatheringItem.ItemNum;
                
                CDataWalletPoint characterWalletPoint = character.WalletPointList.Where(wp => wp.Type == walletTypeAndQuantity.Type).First();
                characterWalletPoint.Value += totalQuantityToAdd; // TODO: Cap to maximum for that wallet

                server.Database.UpdateWalletPoint(character.CharacterId, characterWalletPoint, connectionIn);

                CDataUpdateWalletPoint walletUpdate = new CDataUpdateWalletPoint();
                walletUpdate.Type = walletTypeAndQuantity.Type;
                walletUpdate.AddPoint = (int) totalQuantityToAdd;
                walletUpdate.Value = characterWalletPoint.Value;
                ntc.UpdateWalletList.Add(walletUpdate);
                
                gatheringItem.ItemNum -= pickedGatherItems;
            } else {
                List<CDataItemUpdateResult> results = AddItem(server, character, true, gatheringItem.ItemId, pickedGatherItems, connectionIn:connectionIn);
                ntc.UpdateItemList.AddRange(results);
                gatheringItem.ItemNum -= (uint) results.Select(result => result.UpdateItemNum).Sum();
            }
        }

        public List<CDataItemUpdateResult> ConsumeItemByUIdFromMultipleStorages(DdonServer<GameClient> server, Character character, List<StorageType> fromStorageTypes, string itemUId, uint consumeNum, DbConnection? connectionIn = null)
        {
            int remainingItems = (int) consumeNum;
            List<CDataItemUpdateResult> results = new List<CDataItemUpdateResult>();
            foreach (StorageType storageType in fromStorageTypes)
            {
                CDataItemUpdateResult? result = ConsumeItemByUId(server, character, storageType, itemUId, (uint) remainingItems, connectionIn);
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

        public CDataItemUpdateResult? ConsumeItemByUId(DdonServer<GameClient> server, Character character, StorageType fromStorageType, string itemUId, uint consumeNum, DbConnection? connectionIn = null)
        {
            var foundItem = character.Storage.GetStorage(fromStorageType).FindItemByUId(itemUId);
            if(foundItem == null)
            {
                return null;
            } else {
                (ushort slotNo, Item item, uint itemNum) = foundItem;
                return ConsumeItem(server, character, fromStorageType, slotNo, item, itemNum, consumeNum, connectionIn);
            }
        }

        public CDataItemUpdateResult? ConsumeItemByUIdFromItemBag(DdonServer<GameClient> server, Character character, string itemUId, uint consumeNum, DbConnection? connectionIn = null)
        {
            List<CDataItemUpdateResult> results = ConsumeItemByUIdFromMultipleStorages(server, character, ItemBagStorageTypes, itemUId, consumeNum, connectionIn);
            return results.Count > 0 ? results[0] : null;
        }

        public CDataItemUpdateResult? ConsumeItemByIdFromMultipleStorages(DdonServer<GameClient> server, Character character, List<StorageType> storages, uint itemId, uint consumeNum, DbConnection? connectionIn = null)
        {
            foreach (StorageType storageType in storages)
            {
                var items = character.Storage.GetStorage(storageType).FindItemsById(itemId);
                if (items.Count == 0)
                {
                    continue;
                }

                foreach (var item in items)
                {
                    if (item.Item3 < consumeNum)
                    {
                        continue;
                    }

                    return ConsumeItemByUId(server, character, storageType, item.Item2.UId, consumeNum, connectionIn);
                }
            }

            return null;
        }

        public CDataItemUpdateResult? ConsumeItemByIdFromItemBag(DdonServer<GameClient> server, Character character, uint itemId, uint consumeNum, DbConnection? connectionIn = null)
        {
            return ConsumeItemByIdFromMultipleStorages(server, character, ItemBagStorageTypes, itemId, consumeNum, connectionIn);
        }

        public CDataItemUpdateResult? ConsumeItemInSlot(DdonServer<GameClient> server, Character character, StorageType fromStorageType, ushort slotNo, uint consumeNum, DbConnection? connectionIn = null)
        {
            var foundItem = character.Storage.GetStorage(fromStorageType).GetItem(slotNo);
            if(foundItem == null)
            {
                return null;
            } else {
                (Item item, uint itemNum) = foundItem;
                return ConsumeItem(server, character, fromStorageType, slotNo, item, itemNum, consumeNum, connectionIn);
            }
        }

        private CDataItemUpdateResult ConsumeItem(DdonServer<GameClient> server, Character character, StorageType fromStorageType, ushort slotNo, Item item, uint itemNum, uint consumeNum, DbConnection? connectionIn = null)
        {
            uint finalItemNum = (uint) Math.Max(0, (int)itemNum - (int)consumeNum);
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
            ntcData.ItemList.EquipPoint = item.EquipPoints;
            ntcData.ItemList.EquipCharacterID = 0;
            ntcData.ItemList.EquipPawnID = 0;
            ntcData.ItemList.EquipElementParamList = item.EquipElementParamList;
            ntcData.ItemList.AddStatusParamList = item.AddStatusParamList;
            ntcData.ItemList.Unk2List = item.Unk2List;
            ntcData.UpdateItemNum = -finalConsumeNum;

            Storage fromStorage = character.Storage.GetStorage(fromStorageType);
            if(finalItemNum == 0)
            {
                // Delete item when ItemNum reaches 0 to free up the slot
                fromStorage.SetItem(null, 0, slotNo);
                server.Database.DeleteStorageItem(character.CharacterId, fromStorageType, slotNo, connectionIn);
            }
            else
            {
                fromStorage.SetItem(item, finalItemNum, slotNo);
                server.Database.ReplaceStorageItem(character.CharacterId, fromStorageType, slotNo, finalItemNum, item, connectionIn);
            }

            return ntcData;
        }

        public List<CDataItemUpdateResult> AddItem(DdonServer<GameClient> server, Character character, bool itemBag, uint itemId, uint num, byte plusvalue = 0, DbConnection? connectionIn = null)
        {
            ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, itemId);
            if(itemBag)
            {
                // Limit stacks when adding to the item bag.
                return DoAddItem(server.Database, character, clientItemInfo.StorageType, itemId, num, clientItemInfo.StackLimit, plusvalue, connectionIn);
            }
            else
            {
                // TODO: Support adding to the extension boxes if the storage box is full and the GG course allows it
                if(clientItemInfo.StorageType == StorageType.ItemBagEquipment)
                {
                    // Equipment is a special case. It can't be stacked, even on the storage box. So we limit in there too
                    return DoAddItem(server.Database, character, StorageType.StorageBoxNormal, itemId, num, clientItemInfo.StackLimit, plusvalue, connectionIn);
                }
                else
                {
                    // Move to storage box without stack limit if it's not equipment
                    return DoAddItem(server.Database, character, StorageType.StorageBoxNormal, itemId, num, connectionIn:connectionIn);
                }
            }
        }

        public List<CDataItemUpdateResult> AddItem(DdonServer<GameClient> server, Character character, StorageType destinationStorage, uint itemId, uint num, byte plusvalue = 0, DbConnection? connectionIn = null)
        {
            ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, itemId);
            if (destinationStorage == StorageType.ItemBagConsumable || destinationStorage == StorageType.ItemBagMaterial || destinationStorage == StorageType.ItemBagJob)
            {
                // Limit stacks when adding to the item bag.
                return DoAddItem(server.Database, character, clientItemInfo.StorageType, itemId, num, clientItemInfo.StackLimit, connectionIn:connectionIn);
            }
            else
            {
                // TODO: Support adding to the extension boxes if the storage box is full and the GG course allows it
                if (clientItemInfo.StorageType == StorageType.ItemBagEquipment)
                {
                    // Equipment is a special case. It can't be stacked, even on the storage box. So we limit in there too
                    return DoAddItem(server.Database, character, destinationStorage, itemId, num, clientItemInfo.StackLimit, plusvalue, connectionIn);
                }
                else
                {
                    // Move to storage box without stack limit if it's not equipment
                    return DoAddItem(server.Database, character, destinationStorage, itemId, num, connectionIn:connectionIn);
                }
            }
        }

        private List<CDataItemUpdateResult> DoAddItem(IDatabase database, Character character, StorageType destinationStorageType, uint itemId, uint num, uint stackLimit = UInt32.MaxValue, byte plusvalue = 0, DbConnection? connectionIn = null)
        {
            // Add to existing stacks or make new stacks until there are no more items to add
            // The stack limit is specified by the stackLimit arg
            List<CDataItemUpdateResult> results = new List<CDataItemUpdateResult>();
            uint itemsToAdd = num;
            while(itemsToAdd > 0)
            {
                var itemAndNumWithSlot = character.Storage.GetStorage(destinationStorageType).Items
                    .Select((itemAndCount, index) => new {item = itemAndCount, slot = (ushort) (index + 1)})
                    .Where(itemAndNumWithSlot => (
                        itemAndNumWithSlot.item?.Item1.ItemId == itemId
                        && itemAndNumWithSlot.item?.Item1.PlusValue == plusvalue
                        && itemAndNumWithSlot.item?.Item2 < stackLimit
                    ))
                    .FirstOrDefault();

                Item? item = itemAndNumWithSlot?.item?.Item1;
                ushort slot = itemAndNumWithSlot?.slot ?? 0;
                uint oldItemNum = itemAndNumWithSlot?.item?.Item2 ?? 0;
                uint newItemNum = Math.Min(stackLimit, oldItemNum + itemsToAdd);
                uint addedItems = newItemNum - oldItemNum;
                itemsToAdd -= addedItems;
                
                Storage destinationStorage = character.Storage.GetStorage(destinationStorageType);
                if (item == null)
                {
                    item = new Item() {
                        ItemId = itemId,
                        Unk3 = 0,
                        Color = 0,
                        PlusValue = plusvalue,
                        EquipPoints = 0,
                        EquipElementParamList = new List<CDataEquipElementParam>(),
                        AddStatusParamList = new List<CDataAddStatusParam>(),
                        Unk2List = new List<CDataEquipItemInfoUnk2>()
                    };
                    slot = destinationStorage.AddItem(item, newItemNum);
                }
                else
                {
                    destinationStorage.SetItem(item, newItemNum, slot);
                }

                database.ReplaceStorageItem(character.CharacterId, destinationStorageType, slot, newItemNum, item, connectionIn);
               
                CDataItemUpdateResult result = new CDataItemUpdateResult();
                result.ItemList.ItemUId = item.UId;
                result.ItemList.ItemId = item.ItemId;
                result.ItemList.ItemNum = newItemNum;
                result.ItemList.Unk3 = item.Unk3;
                result.ItemList.StorageType = destinationStorageType;
                result.ItemList.SlotNo = slot;
                result.ItemList.Color = item.Color;
                result.ItemList.PlusValue = item.PlusValue;
                result.ItemList.Bind = false;
                result.ItemList.EquipPoint = item.EquipPoints;
                result.ItemList.EquipCharacterID = 0;
                result.ItemList.EquipPawnID = 0;
                result.ItemList.EquipElementParamList = item.EquipElementParamList;
                result.ItemList.AddStatusParamList = item.AddStatusParamList;
                result.ItemList.Unk2List = item.Unk2List;
                result.UpdateItemNum = (int) addedItems;
                results.Add(result);
            }
            return results;
        }

        public List<CDataItemUpdateResult> MoveItem(DdonServer<GameClient> server, Character character, Storage fromStorage, string itemUId, uint num, Storage toStorage, ushort toSlotNo, DbConnection? connectionIn = null)
        {
            var foundItem = fromStorage.FindItemByUId(itemUId);
            if(foundItem == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_ITEM_NOT_FOUND);
            }

            (ushort fromSlotNo, Item fromItem, uint fromItemNum) = foundItem;
            return MoveItem(server, character, fromStorage, fromSlotNo, num, toStorage, toSlotNo, connectionIn);
        }

        private void DeleteItem(DdonServer<GameClient> server, Character character, Item item, Storage storage, ushort slotNo, DbConnection? connectionIn = null)
        {
            storage.SetItem(null, 0, slotNo);
            server.Database.DeleteStorageItem(character.CharacterId, storage.Type, slotNo, connectionIn);
        }

        private void UpdateItem(DdonServer<GameClient> server, Character character, Item item, Storage storage, ushort slotNo, uint num,DbConnection? connectionIn = null)
        {
            storage.SetItem(item, num, slotNo);
            server.Database.UpdateStorageItem(character.CharacterId, storage.Type, slotNo, num, item, connectionIn);
        }

        private void InsertItem(DdonServer<GameClient> server, Character character, Item item, Storage storage, ushort slotNo, uint num, DbConnection? connectionIn = null)
        {
            storage.SetItem(item, num, slotNo);

            server.Database.InsertStorageItem(character.CharacterId, storage.Type, slotNo, num, item, connectionIn);
            
            foreach (var crest in item.EquipElementParamList)
            {
                server.Database.InsertCrest(character.CommonId, item.UId, crest.SlotNo, crest.CrestId, crest.Add, connectionIn);
            }
        }

        public List<CDataItemUpdateResult> MoveItem(DdonServer<GameClient> server, Character character, Storage fromStorage, ushort fromSlotNo, uint num, Storage toStorage, ushort toSlotNo, DbConnection? connectionIn = null)
        {
            List<CDataItemUpdateResult> results = new List<CDataItemUpdateResult>();

            var toItem = toStorage.GetItem(toSlotNo);
            var fromItem = fromStorage.GetItem(fromSlotNo);
            if (fromItem == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_ITEM_NOT_FOUND);
            }

            if (toStorage.Type == StorageType.CharacterEquipment || toStorage.Type == StorageType.PawnEquipment || toStorage.Type == StorageType.ItemBagEquipment)
            {
                if (toItem != null)
                {
                    // Delete the item
                    DeleteItem(server, character, toItem.Item1, toStorage, toSlotNo, connectionIn);
                }
                DeleteItem(server, character, fromItem.Item1, fromStorage, fromSlotNo, connectionIn);

                if (toItem != null)
                {
                    // Create response which swaps position with the new item being equipped
                    results.Add(CreateItemUpdateResult(character, toItem.Item1, toStorage, toSlotNo, 0, 0));
                    results.Add(CreateItemUpdateResult(null, toItem.Item1, fromStorage, fromSlotNo, 1, 1));

                    InsertItem(server, character, toItem.Item1, fromStorage, fromSlotNo, 1, connectionIn);
                }

                if (toSlotNo == 0)
                {
                    // Going to some type of storage (bag or box)
                    // Find a new slot for the item
                    toSlotNo = toStorage.AddItem(fromItem.Item1, 0);

                    // Create response which places the item in the new location
                    if (fromStorage.Type == StorageType.CharacterEquipment || fromStorage.Type == StorageType.PawnEquipment)
                    {
                        results.Add(CreateItemUpdateResult(character, fromItem.Item1, fromStorage, fromSlotNo, 0, 0));
                    }
                    else
                    {
                        results.Add(CreateItemUpdateResult(null, fromItem.Item1, fromStorage, fromSlotNo, 0, 0));
                    }
                    results.Add(CreateItemUpdateResult(null, fromItem.Item1, toStorage, toSlotNo, 1, 1));
                }
                else
                {
                    // This handles:
                    // - equipment_bag -> equipment
                    // - equipment     -> equipment_bag
                    // - equipment     -> storage
                    // - storage       -> equipment

                    if (fromStorage.Type == StorageType.CharacterEquipment || fromStorage.Type == StorageType.PawnEquipment)
                    {
                        results.Add(CreateItemUpdateResult(character, fromItem.Item1, fromStorage, fromSlotNo, 0, 0));
                    }
                    else
                    {
                        results.Add(CreateItemUpdateResult(null, fromItem.Item1, fromStorage, fromSlotNo, 0, 0));
                    }

                    if (toStorage.Type == StorageType.CharacterEquipment || toStorage.Type == StorageType.PawnEquipment)
                    {
                        results.Add(CreateItemUpdateResult(character, fromItem.Item1, toStorage, toSlotNo, 1, 1));
                    }
                    else
                    {
                        results.Add(CreateItemUpdateResult(null, fromItem.Item1, toStorage, toSlotNo, 1, 1));
                    }
                }
                InsertItem(server, character, fromItem.Item1, toStorage, toSlotNo, 1, connectionIn);
            }
            else
            {
                // Moving items to/from or unequipping an item
                uint newSrcItemNum = fromItem.Item2 - num;
                if (newSrcItemNum == 0)
                {
                    DeleteItem(server, character, fromItem.Item1, fromStorage, fromSlotNo, connectionIn);
                }
                else
                {
                    UpdateItem(server, character, fromItem.Item1, fromStorage, fromSlotNo, newSrcItemNum, connectionIn);
                }

                results.Add(CreateItemUpdateResult(null, fromItem.Item1, fromStorage, fromSlotNo, newSrcItemNum, num));

                uint stackLimit = ItemManager.STACK_BOX_MAX;
                ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, fromItem.Item1.ItemId);
                if (clientItemInfo.StorageType == StorageType.ItemBagEquipment || ItemBagStorageTypes.Contains(toStorage.Type))
                {
                    stackLimit = clientItemInfo.StackLimit;
                }

                uint itemsToMove = num;
                while (itemsToMove > 0)
                {
                    uint oldDstItemNum = 0;
                    ushort dstSlotNo = toSlotNo;

                    Item item = fromItem.Item1;

                    if (toSlotNo == 0)
                    {
                        var itemInDstStorage = toStorage.Items
                            .Select((item, index) => new { item, index })
                            .Where(tuple => fromItem.Item1.ItemId == tuple.item?.Item1.ItemId && tuple.item?.Item2 < stackLimit)
                            .FirstOrDefault();

                        if (itemInDstStorage == null)
                        {
                            // Allocate a new slot to stick these items
                            oldDstItemNum = 0;
                            dstSlotNo = toStorage.AddItem(fromItem.Item1, 0);
                        }
                        else
                        {
                            // There is an existing stack, try to merge them
                            oldDstItemNum = itemInDstStorage.item!.Item2;
                            dstSlotNo = (ushort)(itemInDstStorage.index + 1);
                            item = itemInDstStorage.item!.Item1;
                        }
                    }
                    else
                    {
                        if (toItem != null)
                        {
                            if (toItem.Item1.ItemId != fromItem.Item1.ItemId)
                            {
                                // There is another item in the desired slot but they are not the same
                                // so we need to swap them.
                                results.AddRange(MoveItem(server, character, toStorage, toSlotNo, toItem.Item2, fromStorage, fromSlotNo, connectionIn));
                            }
                            else
                            {
                                oldDstItemNum = toItem.Item2;
                                item = toItem.Item1;
                            }
                        }
                        dstSlotNo = toSlotNo;
                    }

                    uint newDstItemNum = ((oldDstItemNum + itemsToMove) > stackLimit) ? stackLimit : (oldDstItemNum + itemsToMove);
                    uint movedItemNum = newDstItemNum - oldDstItemNum;
                    if (movedItemNum == 0)
                    {
                        // if we move 0 items, this code will get stuck in an infinite loop
                        // break out and report an error so we can investigate it.
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR);
                    }

                    if (clientItemInfo.StorageType != StorageType.ItemBagEquipment)
                    {
                        // Handles stacks being merged or new ones being created
                        item = (oldDstItemNum == 0) ? new Item(item) : item;
                    }

                    toStorage.SetItem(item, newDstItemNum, dstSlotNo);
                    if (oldDstItemNum == 0)
                    {
                        InsertItem(server, character, item, toStorage, dstSlotNo, newDstItemNum, connectionIn);
                    }
                    else
                    {
                        UpdateItem(server, character, item, toStorage, dstSlotNo, newDstItemNum, connectionIn);
                    }
                    results.Add(CreateItemUpdateResult(null, item, toStorage, dstSlotNo, newDstItemNum, movedItemNum));

                    itemsToMove -= movedItemNum;
                }
            }

            return results;
        }

        public CDataItemUpdateResult CreateItemUpdateResult(CharacterCommon character, Item item, StorageType storageType, ushort slotNo, uint itemNum, uint updateItemNum)
        {
            uint pawnId = 0;
            uint characterId = 0;
            if (character is Character)
            {
                characterId = ((Character)character).CharacterId;
            }
            else if (character is Pawn)
            {
                pawnId = ((Pawn)character).PawnId;
            }

            CDataItemUpdateResult updateResult = new CDataItemUpdateResult();
            updateResult.ItemList.ItemUId = item.UId;
            updateResult.ItemList.ItemId = item.ItemId;
            updateResult.ItemList.ItemNum = itemNum;
            updateResult.ItemList.Unk3 = item.Unk3;
            updateResult.ItemList.StorageType = storageType;
            updateResult.ItemList.SlotNo = slotNo;
            updateResult.ItemList.Color = item.Color;
            updateResult.ItemList.PlusValue = item.PlusValue;
            updateResult.ItemList.Bind = false;
            updateResult.ItemList.EquipPoint = item.EquipPoints;
            updateResult.ItemList.EquipCharacterID = characterId;
            updateResult.ItemList.EquipPawnID = pawnId;
            updateResult.ItemList.EquipElementParamList = item.EquipElementParamList;
            updateResult.ItemList.AddStatusParamList = item.AddStatusParamList;
            updateResult.ItemList.Unk2List = item.Unk2List;
            updateResult.UpdateItemNum = (int) updateItemNum;

            return updateResult;
        }

        public CDataItemUpdateResult CreateItemUpdateResult(Character character, Item item, Storage storage, ushort slotNo, uint itemNum, uint updateItemNum)
        {
            return CreateItemUpdateResult(character, item, storage.Type, slotNo, itemNum, updateItemNum);
        }

        public uint LookupItemByUID(DdonServer<GameClient> server, string itemUID, DbConnection? connectionIn = null)
        {
            var item = server.Database.SelectStorageItemByUId(itemUID, connectionIn);
            if (item == null)
            {
                throw new ItemDoesntExistException(itemUID);
            }

            return item.ItemId;
        }

        public void UpgradeStorageItem(DdonGameServer server, GameClient client, UInt32 characterID, StorageType storageType, Item newItem, ushort slotNo, DbConnection? connectionIn = null)
        {
            client.Character.Storage.GetStorage(storageType).SetItem(newItem, 1, slotNo);
            server.Database.UpdateStorageItem(characterID, storageType, slotNo, 1, newItem, connectionIn);

            CDataItemUpdateResult updateResult = new CDataItemUpdateResult();
            updateResult.ItemList.ItemUId = newItem.UId;
            updateResult.ItemList.ItemId = newItem.ItemId;
            updateResult.ItemList.ItemNum = 1;
            updateResult.ItemList.Unk3 = newItem.Unk3;
            updateResult.ItemList.StorageType = storageType;
            updateResult.ItemList.SlotNo = slotNo;
            updateResult.ItemList.Color = newItem.Color;
            updateResult.ItemList.PlusValue = newItem.PlusValue;
            updateResult.ItemList.Bind = false;
            updateResult.ItemList.EquipPoint = newItem.EquipPoints;
            updateResult.ItemList.EquipCharacterID = 0;
            updateResult.ItemList.EquipPawnID = 0;
            updateResult.ItemList.EquipElementParamList = newItem.EquipElementParamList;
            updateResult.ItemList.AddStatusParamList = newItem.AddStatusParamList;
            updateResult.ItemList.Unk2List = newItem.Unk2List;
            updateResult.UpdateItemNum = 1;

            Logger.Debug($"Upgraded {newItem.UId} Item in DataBase");
        }

        public bool HasItem(DdonServer<GameClient> server, Character character, StorageType fromStorage, string itemUId, uint num)
        {
            var foundItem = character.Storage.GetStorage(fromStorage).FindItemByUId(itemUId);
            if (foundItem == null)
            {
                return false;
            }

            return foundItem.Item3 >= num;
        }

        public bool HasItem(DdonServer<GameClient> server, Character character, List<StorageType> storages, string itemUId, uint num)
        {
            uint amountFound = 0;
            foreach (var storage in storages)
            {
                var foundItem = character.Storage.GetStorage(storage).FindItemByUId(itemUId);
                if (foundItem == null)
                {
                    continue;
                }

                amountFound += foundItem.Item3;
            }

            return amountFound >= num;
        }
        public ClientItemInfo LookupInfoByUID(DdonGameServer server, string itemUID, DbConnection? connectionIn = null)
        {
            var item = server.Database.SelectStorageItemByUId(itemUID, connectionIn);
            if (item == null)
            {
                throw new ItemDoesntExistException(itemUID);
            }
            return LookupInfoByItem(server, item);
        }

        public ClientItemInfo LookupInfoByItem(DdonGameServer server, Item item)
        {
            var id = item.ItemId;
            if (!server.AssetRepository.ClientItemInfos.ContainsKey(id))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR);
            }
            return server.AssetRepository.ClientItemInfos[id];
        }

    }

    [Serializable]
    internal class ItemDoesntExistException : Exception
    {
        private string itemUID;

        public ItemDoesntExistException(string itemUID) : base ($"An item with the UID {itemUID} is missing in the database")
        {
            this.itemUID = itemUID;
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
