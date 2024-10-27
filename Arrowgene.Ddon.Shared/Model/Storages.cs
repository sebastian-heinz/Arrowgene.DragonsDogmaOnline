#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Storages
    {
        private Dictionary<StorageType, Storage> storages;

        public Storages(Dictionary<StorageType, ushort> maxSlotsDict)
        {
            storages = new Dictionary<StorageType, Storage>();
            foreach (var tuple in maxSlotsDict)
            {
                AddStorage(tuple.Key, new Storage(tuple.Key, tuple.Value));
            }
        }

        public Dictionary<StorageType, Storage> GetAllStorages()
        {
            return storages;
        }

        public List<CDataCharacterItemSlotInfo> GetAllStoragesAsCDataCharacterItemSlotInfoList()
        {
            return storages
                .Select(storage => new CDataCharacterItemSlotInfo() {
                    StorageType = storage.Key,
                    SlotMax = (ushort) storage.Value.Items.Count
                })
                .ToList();
        }

        public Tuple<StorageType, Tuple<ushort, Item, uint>> FindItemByUIdInStorage(List<StorageType> storageTypes, string uId)
        {
            foreach (var storage in storages)
            {
                var foundItem = storage.Value.FindItemByUId(uId);
                if (foundItem != null)
                {
                    return (storage.Key, foundItem).ToTuple();
                }
            }
            return null;
        }

        public Storage GetStorage(StorageType storageType)
        {
            return storages[storageType];
        }

        public bool HasStorage(StorageType storageType)
        {
            return storages.ContainsKey(storageType);
        }

        public void AddStorage(StorageType storageType, Storage storage)
        {
            storages[storageType] = storage;
        }

        public void Clear()
        {
            foreach (var (type, storage) in storages)
            {
                storage.Clear();
            }
        }

        public void Clear(List<StorageType> storageTypes)
        {
            foreach (var (type, storage) in storages)
            {
                if (storageTypes.Contains(type))
                {
                    storage.Clear();
                }
            }
        }

        public Equipment GetCharacterEquipment()
        {
            return new Equipment(GetStorage(StorageType.CharacterEquipment), 0);
        }

        public Equipment GetPawnEquipment(int pawnIndex)
        {
            int offset = pawnIndex * EquipmentTemplate.TOTAL_EQUIP_SLOTS * 2;
            return new Equipment(GetStorage(StorageType.PawnEquipment), offset);
        }

        public List<CDataItemList> GetStorageAsCDataItemList(Character character, StorageType storageType)
        {
            return GetStorage(storageType).Items
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item != null)
                .Select(tuple => new CDataItemList()
                {
                    ItemUId = tuple.item!.Item1.UId,
                    ItemId = tuple.item.Item1.ItemId,
                    ItemNum = tuple.item.Item2,
                    Unk3 = tuple.item.Item1.Unk3,
                    StorageType = storageType,
                    SlotNo = tuple.slot,
                    Color = tuple.item.Item1.Color,
                    PlusValue = tuple.item.Item1.PlusValue,
                    Bind = true,
                    EquipPoint = tuple.item.Item1.EquipPoints,
                    EquipCharacterID = DetermineCharacterId(character, storageType, tuple.slot),
                    EquipPawnID = DeterminePawnId(character, storageType, tuple.slot),
                    EquipElementParamList = tuple.item.Item1.EquipElementParamList,
                    AddStatusParamList = tuple.item.Item1.AddStatusParamList,
                    Unk2List = tuple.item.Item1.Unk2List
                })
                .ToList();
        }

        public static uint DetermineCharacterId(Character character, StorageType storageType, ushort slot)
        {
            if(storageType == StorageType.CharacterEquipment)
            {
                return character.CharacterId;
            }
            else
            {
                return 0;
            }
        }

        public static uint DeterminePawnId(Character character, StorageType storageType, ushort slot)
        {
            if(storageType == StorageType.PawnEquipment)
            {
                int pawnIndex = slot / (EquipmentTemplate.TOTAL_EQUIP_SLOTS * 2);
                return character.Pawns[pawnIndex].PawnId;
            }
            else
            {
                return 0;
            }
        }
    }

    public class Storage
    {
        public StorageType Type { get; private set; }
        public List<Tuple<Item, uint>?> Items { get; set; }
        public byte[] SortData { get; set; }

        public Storage(StorageType type, ushort slotMax) : this(type, slotMax, new byte[1024])
        {

        }

        public Storage(StorageType type, ushort slotMax, byte[] sortData)
        {
            Type = type;
            Items = Enumerable.Repeat<Tuple<Item, uint>?>(null, slotMax).ToList();
            SortData = sortData;
        }

        public void Clear()
        {
            Items = Enumerable.Repeat<Tuple<Item, uint>?>(null, Items.Count).ToList();
            SortData = new byte[1024];
        }

        public Tuple<Item, uint>? GetItem(ushort slot) {
            if (slot == 0)
            {
                return null;
            }
            return Items[slot - 1];
        }

        public ushort AddItem(Item newItem, uint itemCount) {
            // TODO: Limit itemCount to the item's max stack size in storageType
            // Right now this is being managed by ItemManager
            var tuple = Items
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item == null)
                .First();
            SetItem(newItem, itemCount, tuple.slot);
            return tuple.slot;
        }

        public Tuple<Item, uint>? SetItem(Item? newItem, uint itemCount, ushort slot) {
            if(newItem != null && newItem.ItemId == 0)
            {
                throw new ArgumentException("Item Id can't be 0", "newItem");
            }
            
            // TODO: Limit itemCount to the item ID's max stack size in storageType
            Tuple<Item, uint>? oldItem = GetItem(slot);
            Items[slot-1] = newItem == null
                ? null
                : new Tuple<Item, uint>(newItem, itemCount);
            return oldItem;
        }

        public Tuple<ushort, Item, uint>? FindItemByUId(string itemUId)
        {
            for(int index = 0; index < this.Items.Count; index++)
            {
                var itemAndCount = this.Items[index];
                if(itemAndCount?.Item1.UId == itemUId)
                {
                    return new Tuple<ushort, Item, uint>((ushort)(index+1), itemAndCount.Item1, itemAndCount.Item2);
                }
            }

            return null;
        }

        public List<Tuple<ushort, Item, uint>> FindItemsById(uint itemId)
        {
            var results = new List<Tuple<ushort, Item, uint>>();
            for (int index = 0; index < this.Items.Count; index++)
            {
                var itemAndCount = this.Items[index];
                if (itemAndCount?.Item1.ItemId == itemId)
                {
                    results.Add(new Tuple<ushort, Item, uint>((ushort)(index + 1), itemAndCount.Item1, itemAndCount.Item2));
                }
            }

            return results;
        }

        public ushort EmptySlots()
        {
            ushort emptySlots = 0;

            foreach (var item in Items)
            {
                if (item == null)
                {
                    emptySlots += 1;
                }
            }
            
            return emptySlots;
        }

        public ushort MaxSlots()
        {
            return (ushort) Items.Count;
        }
    }

    public class Equipment
    {
        public Storage Storage { get; private set; }
        public int Offset { get; private set; }

        public Equipment(Storage equipmentStorage, int offset)
        {
            Storage = equipmentStorage;
            Offset = offset;
        }

        public List<Item?> GetItems(EquipType equipType)
        {
            return Storage.Items
                .Skip(Offset + calculateEquipTypeOffset(equipType))
                .Take(EquipmentTemplate.TOTAL_EQUIP_SLOTS)
                .Select(tuple => tuple?.Item1)
                .ToList();
        }

        public ushort GetStorageSlot(EquipType equipType, byte equipSlot)
        {
            return (ushort)(Offset + calculateEquipTypeOffset(equipType) + equipSlot);
        }

        public List<CDataContextEquipData> AsCDataContextEquipData(EquipType equipType)
        {
            // In the context equipment lists, the index is the slot. An element with all info set to 0 has to be in place if a slot is not filled
            return GetItems(equipType)
                .Select(x => x == null ? new CDataContextEquipData() : new CDataContextEquipData()
                {
                    ItemId = (ushort) x.ItemId,
                    ColorNo = x.Color,
                    PlusValue = x.Unk3,
                    EquipElementParamList = x.EquipElementParamList,
                    AddStatusParamList = x.AddStatusParamList
                })
                .ToList();
        }

        public List<CDataEquipItemInfo> AsCDataEquipItemInfo(EquipType equipType)
        {
            return GetItems(equipType)
                .Select((x, index) => new {item = x, slot = (byte)(index+1)})
                .Select(tuple => new CDataEquipItemInfo()
                {
                    ItemId = tuple.item?.ItemId ?? 0,
                    Unk0 = tuple.item?.Unk3 ?? 0,
                    EquipType = equipType,
                    EquipSlot = tuple.slot,
                    Color = tuple.item?.Color ?? 0,
                    PlusValue = tuple.item?.PlusValue ?? 0,
                    EquipElementParamList = tuple.item?.EquipElementParamList ?? new List<CDataEquipElementParam>(),
                    AddStatusParamList = tuple.item?.AddStatusParamList ?? new List<CDataAddStatusParam>(),
                    Unk2List = tuple.item?.Unk2List ?? new List<CDataEquipItemInfoUnk2>()
                })
                .ToList();
        }

        public List<CDataCharacterEquipInfo> AsCDataCharacterEquipInfo(EquipType equipType)
        {
            return GetItems(equipType)
                .Select((x, index) => new {item = x, slot = (byte)(index + 1)})
                .Where(tuple => tuple.item != null)
                .Select(tuple => new CDataCharacterEquipInfo()
                {
                    EquipItemUId = tuple!.item!.UId,
                    EquipType = equipType,
                    EquipCategory = tuple!.slot
                })
                .ToList();
        }

        public List<CDataPresetEquipInfo> AsCDataPresetEquipInfo(EquipType equipType)
        {
            return GetItems(equipType)
                .Select((x, index) => new { item = x, slot = (byte)(index + 1) })
                .Where(tuple => tuple.item != null)
                .Select(tuple => new CDataPresetEquipInfo()
                {
                    ItemId = tuple!.item.ItemId,
                    ItemUId = tuple!.item.UId,
                    EquipSlotNo = tuple!.slot,
                    Color = tuple!.item.Color,
                    PlusValue = tuple!.item.PlusValue,
                    EquipElementParamList = tuple!.item.EquipElementParamList
                })
                .ToList();
        }

        private int calculateEquipTypeOffset(EquipType equipType)
        {
            return equipType == EquipType.Performance ? 0 : EquipmentTemplate.TOTAL_EQUIP_SLOTS;
        }
    }

    // Check nItem::E_STORAGE_TYPE in the PS4 debug symbols for IDs?
    public enum StorageType : byte
    {
        ItemBagConsumable = 0x1,
        ItemBagMaterial = 0x2,
        ItemBagEquipment = 0x3, 
        ItemBagJob = 0x4,
        KeyItems = 0x5,
        StorageBoxNormal = 0x6,
        StorageBoxExpansion = 0x7,
        StorageChestDrawer1 = 0x8, // The one in the Arisen Room
        StorageChestDrawer2 = 0x9,
        StorageChestDrawer3 = 0xA,
        Unk11 = 0xB,
        Unk12 = 0xC,
        ItemPost = 0xD,
        CharacterEquipment = 0xE,
        PawnEquipment = 0xF,
        Unk16 = 0x10,
        Unk17 = 0x11,
        Unk18 = 0x12,
        ReceiveInItemBagCraft = 0x13,
        ReceiveInStorageCraft = 0x14
    }
}
